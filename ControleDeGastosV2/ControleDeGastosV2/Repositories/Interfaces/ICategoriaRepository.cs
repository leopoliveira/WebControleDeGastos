using ControleDeGastosV2.Models.Entities;

namespace ControleDeGastosV2.Repositories.Interfaces
{
    public interface ICategoriaRepository
    {

        Task<List<Categoria>> GetAllAsync(int usuarioId, bool isDespesa=false, bool isReceita=false);

        Task<Categoria> GetByIdAsync(int usuarioId, int id);

        Task<int> CreateOrEditCategoriaAsync(Categoria categoria);

    }
}
