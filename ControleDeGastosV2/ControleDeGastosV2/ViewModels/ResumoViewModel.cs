using ControleDeGastosV2.Models.Entities;

namespace ControleDeGastosV2.ViewModels
{
    public class ResumoViewModel
    {

        public IEnumerable<Carteira> Carteiras { get; set; }

        public IEnumerable<Despesa> Despesas { get; set; }

        public IEnumerable<Receita> Receitas { get; set; }

        public IEnumerable<Receita> ReceitasDoMes { get; set; }

        public IEnumerable<Despesa> DespesasDoMes { get; set; }

    }
}
