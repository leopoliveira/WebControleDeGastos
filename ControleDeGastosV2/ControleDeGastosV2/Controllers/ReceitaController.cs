using ControleDeGastosV2.Models;
using ControleDeGastosV2.Models.Entities;
using ControleDeGastosV2.Repositories.Interfaces;
using ControleDeGastosV2.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ControleDeGastosV2.Controllers
{
    [Authorize]
    public class ReceitaController : Controller
    {

        private readonly IReceitaDespesaRepository _receitaRepository;
        private readonly ICarteiraRepository _carteiraRepository;
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly UserManager<IdentityUser<int>> _userManager;

        public ReceitaController(IReceitaDespesaRepository receitaRepository, ICarteiraRepository carteiraRepository, ICategoriaRepository categoriaRepository, UserManager<IdentityUser<int>> userManager)
        {

            _receitaRepository = receitaRepository;
            _carteiraRepository = carteiraRepository;
            _categoriaRepository = categoriaRepository;
            _userManager = userManager;

        }

        //GET: Receita/Index/5&mes=2
        [HttpGet]
        public async Task<IActionResult> Index(int? cid, int? mes, string carteiraFiltro, string categoriaFiltro, DateTime dataInicial, DateTime dataFinal)//Continuar filtros
        {

            var usuario = await _userManager.GetUserAsync(User);

            if (usuario == null)
            {
                TempData["message"] = Message.Serialize("Por favor, faça login antes de prosseguir!", Types.Error);
                return RedirectToAction("Index", "Usuario");
            }

            ReceitaViewModel receitaViewModel = new ReceitaViewModel
            {
                Categorias = await _categoriaRepository.GetAllAsync(usuario.Id, false, true),
                Carteiras = await _carteiraRepository.GetAllAsync(usuario.Id),
                MesAtual = DateTime.Now.Month
            };

            receitaViewModel.Receitas = await _receitaRepository.GetAllReceitasAsync(usuario.Id);

            if (cid.HasValue && cid.Value > 0)
            {
                receitaViewModel.Receitas = receitaViewModel.Receitas.Where(r => r.CarteiraId == cid.Value);
                
            }
            if (mes.HasValue && mes.Value <= 12 && mes.Value >= 1)
            {
                receitaViewModel.Receitas = receitaViewModel.Receitas.Where(r => r.DataReceita.Month == mes && r.DataReceita.Year == DateTime.Now.Year);
                receitaViewModel.MesAtual = mes.Value;

            }

            if (!String.IsNullOrEmpty(carteiraFiltro))
            {
                receitaViewModel.Receitas = receitaViewModel.Receitas.Where(r => r.Carteira.Nome.Contains(carteiraFiltro));
            }

            if (!String.IsNullOrEmpty(categoriaFiltro))
            {
                receitaViewModel.Receitas = receitaViewModel.Receitas.Where(r => r.Categoria.Nome.Contains(categoriaFiltro));
            }

            if(dataInicial > DateTime.Parse("01/01/0001"))
            {
                receitaViewModel.Receitas = receitaViewModel.Receitas.Where(r => r.DataReceita.CompareTo(dataInicial) >= 0);
            }

            if(dataFinal > DateTime.Parse("01/01/0001") && dataFinal <= DateTime.Now)
            {
                receitaViewModel.Receitas = receitaViewModel.Receitas.Where(r => r.DataReceita.CompareTo(dataFinal.AddDays(1)) < 0);
            }

            return View(receitaViewModel);
        }

        //GET: Receita/Cadastrar/5&cid=3
        [HttpGet]
        public async Task<IActionResult> Cadastrar(int? id, int? cid)
        {

            var usuario = await _userManager.GetUserAsync(User);

            if(usuario == null)
            {
                TempData["message"] = Message.Serialize("Por favor, faça login antes de prosseguir!", Types.Error);
                return RedirectToAction("Index", "Usuario");
            }

            Receita receita = new Receita();

            if (id.HasValue && id.Value > 0)
            {
                receita = await _receitaRepository.GetReceitaByIdAsync(usuario.Id, id.Value);
            }
            else if (cid.HasValue && cid.Value > 0)
            {
                receita.CarteiraId = cid.Value;
            }

            //Troca para ViewModel
            ViewData["Carteiras"] = await _carteiraRepository.GetAllAsync(usuario.Id);
            ViewData["Categorias"] = await _categoriaRepository.GetAllAsync(usuario.Id, false, true);

            return View(receita);
        }

        //POST: Receita/Cadastrar
        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromForm] Receita dadosReceita)
        {
            ModelState.Remove("Categoria");
            ModelState.Remove("Carteira");

            if (ModelState.IsValid)
            {
                int salvouQuantos = await _receitaRepository.CreateOrEditReceitaAsync(dadosReceita);

                if (salvouQuantos > 0)
                {
                    TempData["message"] = Message.Serialize("Receita salva com sucesso!", Types.Info);
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["message"] = Message.Serialize("Erro ao salvar receita, por favor, tente novamente!", Types.Error);
                    return View(dadosReceita);
                }
            }
            else
            {
                TempData["message"] = Message.Serialize("Por favor, confira os dados destacados!", Types.Error);
                return View(dadosReceita);
            }
        }

        //GET: Receita/Deletar
        [HttpGet]
        public async Task<IActionResult> Deletar(int? id)
        {

            var usuario = await _userManager.GetUserAsync(User);

            if(usuario == null)
            {
                TempData["message"] = Message.Serialize("Por favor, faça login antes de prosseguir!", Types.Error);
                return RedirectToAction("Index", "Usuario");
            }

            if (id.HasValue && id.Value > 0)
            {

                Receita receita = await _receitaRepository.GetReceitaByIdAsync(usuario.Id, id.Value);

                return View(receita);
            }
            else
            {
                TempData["message"] = Message.Serialize("Receita não encontrada.", Types.Error);
                return RedirectToAction("Index", "Receita");
            }
        }

        //POST: Receita/Deletar
        [HttpPost]
        public async Task<IActionResult> Deletar([FromForm] Receita dadosReceita)
        {
            int salvouQuantos = await _receitaRepository.DeleteReceitaAsync(dadosReceita);

            if (salvouQuantos > 0)
            {
                TempData["message"] = Message.Serialize("Receita excluída com sucesso!", Types.Info);
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = Message.Serialize("Erro ao excluir receita, por favor, tente novamente!", Types.Error);
                return View(dadosReceita);
            }

        }
    }
}
