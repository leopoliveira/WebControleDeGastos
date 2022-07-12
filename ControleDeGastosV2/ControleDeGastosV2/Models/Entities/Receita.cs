using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControleDeGastosV2.Models.Entities
{
    [Table("Receitas")]
    public class Receita
    {

        [Key]
        public int ReceitaId { get; set; }

        [Required(ErrorMessage = "Necessário descrever a receita.")]
        [StringLength(100, ErrorMessage = "A descrição da receita pode ter no máximo {0} caracteres.")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "Necessário informar o valor da receita.")]
        [Column(TypeName = "decimal(10,2)")]
        [DataType(DataType.Currency)]
        public double Valor { get; set; }

        [Required(ErrorMessage = "Necessário informar a data da receita.")]
        public DateTime DataReceita { get; set; }

        public int CategoriaId { get; set; }

        [ForeignKey("CategoriaId")]
        public Categoria Categoria { get; set; }

        public int CarteiraId { get; set; }

        [ForeignKey("CarteiraId")]
        public Carteira Carteira { get; set; }

    }
}
