using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControleDeGastosV2.Models.Entities
{
    [Table("Categorias")]
    public class Categoria
    {

        [Key]
        public int CategoriaId { get; set; }

        [Required(ErrorMessage = "Necessário dar um nome para a categoria.")]
        [StringLength(32, ErrorMessage = "O nome da categoria pode ter no máximo {0} caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Necessário definir uma cor para a categoria.")]
        [StringLength(10)]
        public string CorHexaDecimal { get; set; }

        [Display(Name = "Pode ser usado para receita?")]
        public bool IsReceita { get; set; }

        [Display(Name = "Pode ser usado para despesas?")]
        public bool IsDepesa { get; set; }

        public int UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        public IdentityUser<int> Usuario { get; set; }

    }
}
