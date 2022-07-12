using ControleDeGastosV2.Models.Entities;

namespace ControleDeGastosV2.ViewModels
{
    public class DespesaViewModel
    {

        public IEnumerable<Despesa> Despesas { get; set; }

        public IEnumerable<Carteira> Carteiras { get; set; }

        public IEnumerable<Categoria> Categorias { get; set; }

        public int MesAtual { get; set; }
    }
}
