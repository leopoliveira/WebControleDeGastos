using System.ComponentModel.DataAnnotations;

namespace ControleDeGastosV2.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Necessário informar o nome de usuário.")]
        public string Usuario { get; set; }

        [Required(ErrorMessage = "Necessário informar a senha.")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Necessário confimar a senha.")]
        [Compare(nameof(Senha), ErrorMessage = "As senhas não conferem.")]
        public string ConfirmarSenha { get; set; }

        public bool LembraDeMim { get; set; }

        public string ReturnUrl { get; set; }

    }
}
