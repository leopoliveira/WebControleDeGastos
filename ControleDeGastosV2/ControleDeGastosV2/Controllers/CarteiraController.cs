using ControleDeGastosV2.Models;
using ControleDeGastosV2.Models.Entities;
using ControleDeGastosV2.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ControleDeGastosV2.Controllers
{
    [Authorize]
    public class CarteiraController : Controller
    {
        private readonly ICarteiraRepository _carteiraRepository;
        private readonly UserManager<IdentityUser<int>> _userManager;

        public CarteiraController(ICarteiraRepository carteiraRepository, UserManager<IdentityUser<int>> userManager)
        {
            _carteiraRepository = carteiraRepository;
            _userManager = userManager;
        }

        //GET: Carteira/Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {

            var usuario = await _userManager.GetUserAsync(User);

            if(usuario != null)
            {
                IEnumerable<Carteira> carteiras = await _carteiraRepository.GetAllAsync(usuario.Id);

                return View(carteiras);
            }

            TempData["message"] = Message.Serialize("Por favor, faça login antes de prosseguir!", Types.Error);
            return RedirectToAction("Index", "Usuario");

        }

        //GET: Carteira/Cadastrar/5
        [HttpGet]
        public async Task<IActionResult> Cadastrar(int? id)
        {
            Carteira carteira = new Carteira();

            var usuario = await _userManager.GetUserAsync(User);

            if(usuario != null)
            {
                if (id.HasValue && id.Value > 0)
                {
                    carteira = await _carteiraRepository.GetByIdAsync(usuario.Id, id.Value);
                }
            }

            return View(carteira);
            
        }

        //POST: Carteira/Cadastrar
        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromForm] Carteira dadosCarteira)
        {
            var usuario = await _userManager.GetUserAsync(User);

            if (usuario == null)
            {
                TempData["message"] = Message.Serialize("Necessário fazer login!", Types.Error);
                return View("Index", "Usuario");
            }

            if (ModelState.IsValid)
            {
                dadosCarteira.UsuarioId = usuario.Id;

                int salvouQuantos = await _carteiraRepository.CreateOrEditCarteiraAsync(dadosCarteira);

                if(salvouQuantos > 0)
                {
                    TempData["message"] = Message.Serialize("Carteira salva com sucesso!", Types.Info);
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["message"] = Message.Serialize("Erro ao salvar carteira, por favor, tente novamente!", Types.Error);
                    return View(dadosCarteira);
                }

            }
            else
            {
                TempData["message"] = Message.Serialize("Por favor, confira os dados destacados!", Types.Error);
                return View(dadosCarteira);
            }
        }
    }
}