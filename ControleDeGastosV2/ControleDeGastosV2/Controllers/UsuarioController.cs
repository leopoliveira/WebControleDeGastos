using ControleDeGastosV2.Models;
using ControleDeGastosV2.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ControleDeGastosV2.Controllers
{
    public class UsuarioController : Controller
    {

        private readonly UserManager<IdentityUser<int>> _userManager;
        private readonly SignInManager<IdentityUser<int>> _signInManager;

        public UsuarioController(UserManager<IdentityUser<int>> userManager, SignInManager<IdentityUser<int>> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        //GET: Usuario/Index/retUrl=sssss
        [HttpGet]
        public IActionResult Index(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Resumo");
            }

            LoginViewModel loginVM = new LoginViewModel
            {
                ReturnUrl = returnUrl
            };

            return View(loginVM);
        }

        //POST: Usuario/Login
        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginViewModel dadosLogin)
        {
            ModelState.Remove("ConfirmarSenha");

            if (ModelState.IsValid)
            {
                IdentityUser<int> usuario = await _userManager.FindByNameAsync(dadosLogin.Usuario);

                if(usuario != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(usuario, dadosLogin.Senha, true, false);

                    if (result.Succeeded)
                    {
                        TempData["message"] = Message.Serialize("Logado com sucesso, seja bem-vindo!", Types.Info);

                        if (string.IsNullOrEmpty(dadosLogin.ReturnUrl))
                        {
                            return RedirectToAction("Index", "Resumo");
                        }

                        if (Url.IsLocalUrl(dadosLogin.ReturnUrl))
                        {
                            return Redirect(dadosLogin.ReturnUrl);
                        }
                        else
                        {
                            RedirectToAction("Index", "Resumo");
                        }
                        
                    }
                }

                TempData["message"] = Message.Serialize("Verifique seus dados!", Types.Error);

                return View("Index", dadosLogin);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Erro ao fazer login");

                return View("Index", dadosLogin);
            }
        }

        public async Task<IActionResult> Logout()
        {
            //HttpContext.Session.Clear();
            //HttpContext.User = null;

            await _signInManager.SignOutAsync();

            TempData["message"] = Message.Serialize("Logout efetuado com sucesso... Até a próxima!", Types.Info);

            return View("Index");
        }

        //GET: Usuario/Cadastrar
        [HttpGet]
        public IActionResult Cadastrar()
        {
            return View(new LoginViewModel());
        }

        //POST: Usuario/Cadastrar
        [HttpPost]
        //Validação para evitar ataques ao servidor
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cadastrar([FromForm] LoginViewModel dadosUsuario)
        {
            bool usuarioExiste = _userManager.Users.Any(u => u.UserName == dadosUsuario.Usuario);

            if(usuarioExiste)
            {
                ModelState.AddModelError("", "Usuário já cadastrado!");
                TempData["message"] = Message.Serialize("Erro ao se registrar! Usuário já cadastrado", Types.Error);
                return View(dadosUsuario);
            }

            if (ModelState.IsValid)
            {
                var usuario = new IdentityUser<int>()
                {
                    UserName = dadosUsuario.Usuario
                };

                var result = await _userManager.CreateAsync(usuario, dadosUsuario.Senha);

                if (result.Succeeded)
                {
                    TempData["message"] = Message.Serialize("Cadastro realizado com sucesso!", Types.Info);
                    return RedirectToAction("Index", "Usuario");
                }
                else
                {
                    TempData["message"] = Message.Serialize("Erro ao se registrar.", Types.Error);
                    ModelState.AddModelError("", "Erro ao criar usuário.");
                }
            }

            return View(dadosUsuario);
        }
        
    }
}
