using ControleDeGastosV2.Models;
using ControleDeGastosV2.Models.Entities;
using ControleDeGastosV2.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ControleDeGastosV2.Controllers
{
    [Authorize]
    public class CategoriaController : Controller
    {

        private readonly ICategoriaRepository _categoriaRepository;
        private readonly UserManager<IdentityUser<int>> _userManager;

        public CategoriaController(ICategoriaRepository categoriaRepository, UserManager<IdentityUser<int>> userManager)
        {
            _categoriaRepository = categoriaRepository;
            _userManager = userManager;
        }

        //GET: Categoria/Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {

            var usuario = await _userManager.GetUserAsync(User); 

            if(usuario != null)
            {
                IEnumerable<Categoria> categorias = await _categoriaRepository.GetAllAsync(usuario.Id);

                return View(categorias);
            }

            TempData["message"] = Message.Serialize("Por favor, faça login antes de prosseguir!", Types.Error);
            return RedirectToAction("Index", "Usuario");
            
        }

        //GET: Categoria/Cadastrar/5
        [HttpGet]
        public async Task<IActionResult> Cadastrar(int? id)
        {

            Categoria categoria = new Categoria();

            var usuario = await _userManager.GetUserAsync(User);

            if(usuario != null)
            {
                if (id.HasValue && id.Value > 0)
                {
                    categoria = await _categoriaRepository.GetByIdAsync(usuario.Id, id.Value);
                }
                
            }

            return View(categoria);

        }

        //POST: Categoria/Cadastrar
        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromForm] Categoria dadosCategoria)
        {

            var usuario = await _userManager.GetUserAsync(User);

            if (usuario == null)
            {
                TempData["message"] = Message.Serialize("Necessário fazer login!", Types.Error);
                return View("Index", "Usuario");
            }
                

            if (ModelState.IsValid)
            {
                dadosCategoria.UsuarioId = usuario.Id;

                int salvouQuantos = await _categoriaRepository.CreateOrEditCategoriaAsync(dadosCategoria);

                if (salvouQuantos > 0)
                {
                    TempData["message"] = Message.Serialize("Categoria salva com sucesso!", Types.Info);
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["message"] = Message.Serialize("Erro ao salvar categoria, por favor, tente novamente!", Types.Error);
                    return View(dadosCategoria);
                }
            }
            else
            {
                TempData["message"] = Message.Serialize("Por favor, confira os dados destacados!", Types.Error);
                return View(dadosCategoria);
            }

        }

        /*[HttpGet]
        public async Task<IActionResult> Deletar(int? id)
        {

            if(id.HasValue && id.Value > 0)
            {
                Categoria categoria = await _categoriaRepository.GetByIdAsync(id.Value);

                return View(categoria);
            }
            else
            {
                TempData["message"] = Message.Serialize("Categoria não encontrada.", Types.Error);

                return RedirectToAction("Index", "Categoria");
            }

        }*/

    }
}
