using ControleDeGastosV2.Contexts;
using ControleDeGastosV2.Models.Entities;
using ControleDeGastosV2.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ControleDeGastosV2.Repositories
{
    public class CarteiraRepository : ICarteiraRepository
    {

        private readonly Contexto _contexto;

        public CarteiraRepository(Contexto contexto)
        {
            _contexto = contexto;
        }

        public async Task<List<Carteira>> GetAllAsync(int usuarioId)
        {
            return await _contexto.Carteiras
                .Where(c => c.UsuarioId == usuarioId)
                .ToListAsync();
        }

        public async Task<Carteira> GetByIdAsync(int usuarioId, int id)
        {
            return await _contexto.Carteiras.FirstOrDefaultAsync(c => c.CarteiraId == id && c.UsuarioId == usuarioId);
        }

        public async Task<int> CreateOrEditCarteiraAsync(Carteira carteira)
        {
            bool existe = _contexto.Carteiras.Any(c => c.CarteiraId == carteira.CarteiraId);

            if (existe)
            {
                _contexto.Carteiras.Update(carteira);
            }
            else
            {
                _contexto.Carteiras.Add(carteira);
            }

            return await _contexto.SaveChangesAsync();
        }
    }
}
