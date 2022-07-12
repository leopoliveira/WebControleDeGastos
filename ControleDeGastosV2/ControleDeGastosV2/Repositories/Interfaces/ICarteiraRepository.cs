using ControleDeGastosV2.Models.Entities;

namespace ControleDeGastosV2.Repositories.Interfaces
{
    public interface ICarteiraRepository
    {

        Task<List<Carteira>> GetAllAsync(int usuarioId);

        Task<Carteira> GetByIdAsync(int usuarioId, int id);

        Task<int> CreateOrEditCarteiraAsync(Carteira carteira);

    }
}
