using ControleDeGastosV2.Models.Entities;

namespace ControleDeGastosV2.Repositories.Interfaces
{
    public interface IResumoRepository
    {

        Task<List<Carteira>> GetAllCarteirasAsync(int usuarioId);

        Task<List<Categoria>> GetAllCategoriasAsync(int usuarioId);

        Task<List<Receita>> GetAllReceitasAsync(int usuarioId);

        Task<List<Despesa>> GetAllDespesasAsync(int usuarioId);
    }
}
