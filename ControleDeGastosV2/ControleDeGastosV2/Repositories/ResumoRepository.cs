using ControleDeGastosV2.Contexts;
using ControleDeGastosV2.Models.Entities;
using ControleDeGastosV2.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ControleDeGastosV2.Repositories
{
    public class ResumoRepository : IResumoRepository
    {

        private readonly Contexto _contexto;

        public ResumoRepository(Contexto contexto)
        {
            _contexto = contexto;
        }

        public async Task<List<Carteira>> GetAllCarteirasAsync(int usuarioId)
        {
            return await _contexto.Carteiras
                        .Where(cart => cart.UsuarioId == usuarioId)
                        .AsNoTracking()
                        .ToListAsync();
        }

        public async Task<List<Categoria>> GetAllCategoriasAsync(int usuarioId)
        {
            return await _contexto.Categorias
                        .Where(cat => cat.UsuarioId == usuarioId)
                        .AsNoTracking()
                        .ToListAsync();
        }

        public async Task<List<Despesa>> GetAllDespesasAsync(int usuarioId)
        {
            return await _contexto.Despesas
                        .Where(d => d.Carteira.UsuarioId == usuarioId)
                        .AsNoTracking()
                        .ToListAsync();
        }

        public async Task<List<Receita>> GetAllReceitasAsync(int usuarioId)
        {
            return await _contexto.Receitas
                        .Where(r => r.Carteira.UsuarioId == usuarioId)
                        .AsNoTracking()
                        .ToListAsync();
        }
    }
}
