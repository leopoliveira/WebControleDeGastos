using ControleDeGastosV2.Models.Entities;

namespace ControleDeGastosV2.Repositories.Interfaces
{
    public interface IReceitaDespesaRepository
    {

        //Métodos para Receitas

        Task<List<Receita>> GetAllReceitasAsync(int usuarioId);

        Task<Receita> GetReceitaByIdAsync(int usuarioId, int id);

        Task<List<Receita>> GetReceitasByCarteiraAsync(int usuarioId, int carteiraId);

        Task<List<Receita>> GetReceitasByCategoriaAsync(int usuarioId, int categoriaId);

        Task<List<Receita>> GetReceitasMesAsync(int usuarioId, int mes);

        Task<List<Receita>> GetAllReceitasPeriodoAsync(int usuarioId, DateTime inicio, DateTime fim);

        Task<int> CreateOrEditReceitaAsync(Receita receita);

        Task<int> DeleteReceitaAsync(Receita receita);


        //Métodos para Despesas


        Task<List<Despesa>> GetAllDespesasAsync(int usuarioId);

        Task<Despesa> GetDespesaByIdAsync(int usuarioId, int id);

        Task<List<Despesa>> GetDespesasByCarteiraAsync(int usuarioId, int carteiraId);

        Task<List<Despesa>> GetDespesasByCategoriaAsync(int usuarioId, int categoriaId);

        Task<List<Despesa>> GetDespesasMesAsync(int usuarioId, int mes);

        Task<List<Despesa>> GetAllDespesasPeriodoAsync(int usuarioId, DateTime inicio, DateTime fim);

        Task<int> CreateOrEditDespesaAsync(Despesa despesa);

        Task<int> DeleteDespesaAsync(Despesa despesa);

    }
}
