using ControleDeGastosV2.Contexts;
using ControleDeGastosV2.Models.Entities;
using ControleDeGastosV2.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ControleDeGastosV2.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly Contexto _contexto;

        public CategoriaRepository(Contexto contexto)
        {
            _contexto = contexto;
        }

        public async Task<List<Categoria>> GetAllAsync(int usuarioId, bool isDespesa=false, bool isReceita=false)
        {
           if(isDespesa && !isReceita)
            {
                return await _contexto.Categorias
                            .Where(cat => cat.IsDepesa == true && cat.UsuarioId == usuarioId)
                            .AsNoTracking()
                            .ToListAsync();
            }
            else if(!isDespesa && isReceita)
            {
                return await _contexto.Categorias
                            .Where(cat => cat.IsReceita == true && cat.UsuarioId == usuarioId)
                            .AsNoTracking()
                            .ToListAsync();
            }
            else
            {
                return await _contexto.Categorias
                            .Where(cat => cat.UsuarioId == usuarioId)
                            .AsNoTracking()
                            .ToListAsync();
            }
        }

        public async Task<Categoria> GetByIdAsync(int usuarioId, int id)
        {
            return await _contexto.Categorias.FirstOrDefaultAsync(c => c.CategoriaId == id && c.UsuarioId == usuarioId);
        }

        public async Task<int> CreateOrEditCategoriaAsync(Categoria categoria)
        {
            bool existe = _contexto.Categorias.Any(c => c.CategoriaId == categoria.CategoriaId);

            if (existe)
            {
                _contexto.Categorias.Update(categoria);
            }
            else
            {
                _contexto.Categorias.Add(categoria);
            }

            return await _contexto.SaveChangesAsync();
        }
    }
}
