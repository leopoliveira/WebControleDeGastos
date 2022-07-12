using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControleDeGastosV2.Models.Entities
{
    [Table("Depesas")]
    public class Despesa
    {

        [Key]
        public int DespesaId { get; set; }

        [Required(ErrorMessage = "Necessário descrever a despesa.")]
        [StringLength(100, ErrorMessage = "A descrição da despesa pode ter no máximo {0} caracteres.")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "Necessário informar o valor da despesa.")]
        [Column(TypeName = "decimal(10,2)")]
        [DataType(DataType.Currency)]
        public double Valor { get; set; }

        [Required(ErrorMessage = "Necessário informar a data da despesa.")]
        public DateTime DataDespesa { get; set; }

        public int CategoriaId { get; set; }

        [ForeignKey("CategoriaId")]
        public Categoria Categoria { get; set; }

        public int CarteiraId { get; set; }

        [ForeignKey("CarteiraId")]
        public Carteira Carteira { get; set; }

    }
}
