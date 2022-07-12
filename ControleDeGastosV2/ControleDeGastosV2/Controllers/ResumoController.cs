using ControleDeGastosV2.Models;
using ControleDeGastosV2.Repositories.Interfaces;
using ControleDeGastosV2.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ControleDeGastosV2.Controllers
{
    [Authorize]
    public class ResumoController : Controller
    {

        private readonly IResumoRepository _resumoRepository;
        private readonly UserManager<IdentityUser<int>> _userManager;

        public ResumoController(IResumoRepository resumoRepository, UserManager<IdentityUser<int>> userManager)
        {
            _resumoRepository = resumoRepository;
            _userManager = userManager;
        }

        //[Authorize] //Permite apenas que usuário autorizados tenham acesso a essa action
        //GET: Resumo/Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var usuario = await _userManager.GetUserAsync(User);

            if (usuario == null)
            {
                TempData["message"] = Message.Serialize("Necessário fazer login!", Types.Error);
                return View("Index", "Usuario");
            }

            ResumoViewModel resumoVM = new ResumoViewModel
            {
                Carteiras = await _resumoRepository.GetAllCarteirasAsync(usuario.Id),
                Despesas = await _resumoRepository.GetAllDespesasAsync(usuario.Id),
                Receitas = await _resumoRepository.GetAllReceitasAsync(usuario.Id)
            };

            return View(resumoVM);

        }
    }
}
