using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControleDeGastosV2.Models.Entities
{
    [Table("Carteiras")]
    public class Carteira
    {

        [Key]
        public int CarteiraId { get; set; }

        [Required(ErrorMessage = "Necessário dar um nome para a carteira.")]
        [StringLength(32, ErrorMessage = "O nome da carteira pode ter no máximo {0} caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Informe o saldo inicial da carteira.")]
        [Column(TypeName = "decimal(10,2)")]
        [DataType(DataType.Currency)]
        public double Saldo { get; set; }

        [Required(ErrorMessage = "Necessário definir uma cor para a carteira.")]
        [StringLength(10)]
        public string CorHexaDecimal { get; set; }

        public int UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        public IdentityUser<int> Usuario { get; set; }

        [Display(Name = "É carteira principal?")]
        public bool CarteiraPrincipal { get; set; }

    }

}
