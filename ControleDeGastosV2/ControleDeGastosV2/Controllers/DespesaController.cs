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
    public class DespesaController : Controller
    {

        private readonly IReceitaDespesaRepository _despesaRepository;
        private readonly ICarteiraRepository _carteiraRepository;
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly UserManager<IdentityUser<int>> _userManager;

        public DespesaController(IReceitaDespesaRepository despesaRepository, ICarteiraRepository carteiraRepository, ICategoriaRepository categoriaRepository, UserManager<IdentityUser<int>> userManager)
        {

            _despesaRepository = despesaRepository;
            _carteiraRepository = carteiraRepository;
            _categoriaRepository = categoriaRepository;
            _userManager = userManager;

        }

        //GET: Despesa/Index/5&mes=2
        [HttpGet]
        public async Task<IActionResult> Index(int? cid, int? mes, string carteiraFiltro, string categoriaFiltro, DateTime dataInicial, DateTime dataFinal)//Continuar filtros
        {

            var usuario = await _userManager.GetUserAsync(User);

            if (usuario == null)
            {
                TempData["message"] = Message.Serialize("Por favor, faça login antes de prosseguir!", Types.Error);
                return RedirectToAction("Index", "Usuario");
            }

            DespesaViewModel despesaViewModel = new DespesaViewModel
            {
                Categorias = await _categoriaRepository.GetAllAsync(usuario.Id, true, false),
                Carteiras = await _carteiraRepository.GetAllAsync(usuario.Id),
                MesAtual = DateTime.Now.Month
            };

            despesaViewModel.Despesas = await _despesaRepository.GetAllDespesasAsync(usuario.Id);

            if (cid.HasValue && cid.Value > 0)
            {
                despesaViewModel.Despesas = despesaViewModel.Despesas.Where(d => d.CarteiraId == cid.Value);

            }
            if (mes.HasValue && mes.Value <= 12 && mes.Value >= 1)
            {
                despesaViewModel.Despesas = despesaViewModel.Despesas.Where(d => d.DataDespesa.Month == mes && d.DataDespesa.Year == DateTime.Now.Year);
                despesaViewModel.MesAtual = mes.Value;

            }

            if (!String.IsNullOrEmpty(carteiraFiltro))
            {
                despesaViewModel.Despesas = despesaViewModel.Despesas.Where(d => d.Carteira.Nome.Contains(carteiraFiltro));
            }

            if (!String.IsNullOrEmpty(categoriaFiltro))
            {
                despesaViewModel.Despesas = despesaViewModel.Despesas.Where(d => d.Categoria.Nome.Contains(categoriaFiltro));
            }

            if (dataInicial > DateTime.Parse("01/01/0001"))
            {
                despesaViewModel.Despesas = despesaViewModel.Despesas.Where(d => d.DataDespesa.CompareTo(dataInicial) >= 0);
            }

            if (dataFinal > DateTime.Parse("01/01/0001") && dataFinal <= DateTime.Now)
            {
                despesaViewModel.Despesas = despesaViewModel.Despesas.Where(d => d.DataDespesa.CompareTo(dataFinal.AddDays(1)) < 0);
            }

            return View(despesaViewModel);
        }

        //GET: Despesa/Cadastrar/5&cid=3
        [HttpGet]
        public async Task<IActionResult> Cadastrar(int? id, int? cid)
        {

            var usuario = await _userManager.GetUserAsync(User);

            if(usuario == null)
            {
                TempData["message"] = Message.Serialize("Por favor, faça login antes de prosseguir!", Types.Error);
                return RedirectToAction("Index", "Usuario");
            }

            Despesa despesa = new Despesa();

            if (id.HasValue && id.Value > 0)
            {
                despesa = await _despesaRepository.GetDespesaByIdAsync(usuario.Id, id.Value);
            }
            else if (cid.HasValue && cid.Value > 0)
            {
                despesa.CarteiraId = cid.Value;
            }

            //Troca para ViewModel
            ViewData["Carteiras"] = await _carteiraRepository.GetAllAsync(usuario.Id);
            ViewData["Categorias"] = await _categoriaRepository.GetAllAsync(usuario.Id, true, false);

            return View(despesa);
        }

        //POST: Despesa/Cadastrar
        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromForm] Despesa dadosDespesa)
        {
            ModelState.Remove("Categoria");
            ModelState.Remove("Carteira");

            if (ModelState.IsValid)
            {
                int salvouQuantos = await _despesaRepository.CreateOrEditDespesaAsync(dadosDespesa);

                if (salvouQuantos > 0)
                {
                    TempData["message"] = Message.Serialize("Despesa salva com sucesso!", Types.Info);
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["message"] = Message.Serialize("Erro ao salvar despesa, por favor, tente novamente!", Types.Error);
                    return View(dadosDespesa);
                }
            }
            else
            {
                TempData["message"] = Message.Serialize("Por favor, confira os dados destacados!", Types.Error);
                return View(dadosDespesa);
            }
            
        }

        //GET: Despesa/Deletar/5
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

                Despesa despesa = await _despesaRepository.GetDespesaByIdAsync(usuario.Id, id.Value);
                
                return View(despesa);
            }
            else
            {
                TempData["message"] = Message.Serialize("Despesa não encontrada.", Types.Error);
                return RedirectToAction("Index", "Despesa");
            }
            
        }

        //POST: Despesa/Deletar
        [HttpPost]
        public async Task<IActionResult> Deletar([FromForm] Despesa dadosDespesa)
        {
            int salvouQuantos = await _despesaRepository.DeleteDespesaAsync(dadosDespesa);

            if (salvouQuantos > 0)
            {
                TempData["message"] = Message.Serialize("Despesa excluída com sucesso!", Types.Info);
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = Message.Serialize("Erro ao excluir despesa, por favor, tente novamente!", Types.Error);
                return View(dadosDespesa);
            }
        }
    }
}
