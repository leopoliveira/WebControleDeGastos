using ControleDeGastosV2.Contexts;
using ControleDeGastosV2.Models.Entities;
using ControleDeGastosV2.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ControleDeGastosV2.Repositories
{
    public class ReceitaDespesaRepository : IReceitaDespesaRepository
    {

        private readonly Contexto _contexto;


        //Métodos para Receitas


        public ReceitaDespesaRepository(Contexto contexto)
        {
            _contexto = contexto;
        }

        public async Task<List<Receita>> GetAllReceitasAsync(int usuarioId)
        {
            return await _contexto.Receitas
                        .Where(r => r.Carteira.UsuarioId == usuarioId)
                        .Include(r => r.Categoria)
                        .Include(r => r.Carteira)
                        .OrderByDescending(r => r.DataReceita)
                        .ToListAsync();
        }

        public async Task<List<Receita>> GetReceitasByCarteiraAsync(int usuarioId, int carteiraId)
        {
            return await _contexto.Receitas
                        .Where(r => r.CarteiraId == carteiraId && r.Carteira.UsuarioId == usuarioId)
                        .Include(r => r.Categoria)
                        .Include(r => r.Carteira)
                        .OrderByDescending(r => r.DataReceita)
                        .ToListAsync();
        }

        public async Task<List<Receita>> GetReceitasByCategoriaAsync(int usuarioId, int categoriaId)
        {
            return await _contexto.Receitas
                        .Where(c => c.CategoriaId == categoriaId && c.Carteira.UsuarioId == usuarioId)
                        .Include(c => c.Categoria)
                        .Include(r => r.Carteira)
                        .OrderByDescending(r => r.DataReceita)
                        .ToListAsync();
        }

        public async Task<Receita> GetReceitaByIdAsync(int usuarioId, int id)
        {
            return await _contexto.Receitas
                        .Include(r => r.Categoria)
                        .Include(r => r.Carteira)
                        .FirstOrDefaultAsync(r => r.ReceitaId == id && r.Carteira.UsuarioId == usuarioId);
        }

        public async Task<List<Receita>> GetReceitasMesAsync(int usuarioId, int mes)
        {
            return await _contexto.Receitas
                        .Where(r => r.Carteira.UsuarioId == usuarioId && r.DataReceita.Month == mes)
                        .Include(r => r.Categoria)
                        .Include(r => r.Carteira)
                        .OrderBy(r => r.DataReceita)
                        .ToListAsync();
        }

        public async Task<List<Receita>> GetAllReceitasPeriodoAsync(int usuarioId, DateTime inicio, DateTime fim)
        {

            return await _contexto.Receitas
                        .Where(r => r.Carteira.CarteiraId == usuarioId &&
                                DateTime.Compare(r.DataReceita, inicio.AddDays(-1)) > 0 &&
                                DateTime.Compare(r.DataReceita, fim.AddDays(1)) < 0
                               )
                        .AsNoTracking()
                        .ToListAsync();
        }

        public async Task<int> CreateOrEditReceitaAsync(Receita dadosReceita)
        {
            var receita = await _contexto.Receitas
                          .AsNoTracking()
                          .FirstOrDefaultAsync(r => r.ReceitaId == dadosReceita.ReceitaId);

            if (receita != null)
            {

                double novoValor = dadosReceita.Valor - receita.Valor;
                AdicionaSaldoCarteira(novoValor, dadosReceita.CarteiraId);

                _contexto.Receitas.Update(dadosReceita);
            }
            else
            {
                AdicionaSaldoCarteira(dadosReceita.Valor, dadosReceita.CarteiraId);
                _contexto.Receitas.Add(dadosReceita);
            }

            return await _contexto.SaveChangesAsync();
        }

        public async Task<int> DeleteReceitaAsync(Receita receita)
        {

            bool existe = _contexto.Receitas.Any(r => r.ReceitaId == receita.ReceitaId);

            if (existe)
            {
                DiminuindoSaldoCarteira(receita.Valor, receita.CarteiraId);

                _contexto.Receitas.Remove(receita);

                return await _contexto.SaveChangesAsync();
            }
            else
            {
                return 0;
            }

        }

        private void AdicionaSaldoCarteira(double valor, int carteiraId)
        {
            var carteira = _contexto.Carteiras
                            .AsNoTracking()
                           .FirstOrDefault(c => c.CarteiraId == carteiraId);

            carteira.Saldo += valor;

            _contexto.Update(carteira);

            try
            {
                _contexto.SaveChanges();
            }
            catch(DbUpdateException e)
            {
                throw e;
            }

        }


        //Métodos para Despesas


        public async Task<List<Despesa>> GetAllDespesasAsync(int usuarioId)
        {
            return await _contexto.Despesas
                        .Where(d => d.Carteira.UsuarioId == usuarioId)
                        .Include(d => d.Categoria)
                        .Include(d => d.Carteira)
                        .OrderByDescending(d => d.DataDespesa)
                        .ToListAsync();
        }

        public async Task<List<Despesa>> GetDespesasByCarteiraAsync(int usuarioId, int carteiraId)
        {
            return await _contexto.Despesas
                        .Where(d => d.CarteiraId == carteiraId && d.Carteira.UsuarioId == usuarioId)
                        .Include(d => d.Categoria)
                        .Include(d => d.Carteira)
                        .OrderByDescending(d => d.DataDespesa)
                        .ToListAsync();
        }

        public async Task<List<Despesa>> GetDespesasByCategoriaAsync(int usuarioId, int categoriaId)
        {
            return await _contexto.Despesas
                        .Where(d => d.CategoriaId == categoriaId && d.Carteira.UsuarioId == usuarioId)
                        .Include(d => d.Categoria)
                        .Include(d => d.Carteira)
                        .OrderByDescending(d => d.DataDespesa)
                        .ToListAsync();
        }

        public async Task<Despesa> GetDespesaByIdAsync(int usuarioId, int id)
        {
            return await _contexto.Despesas
                        .Include(d => d.Categoria)
                        .Include(d => d.Carteira)
                        .FirstOrDefaultAsync(d => d.DespesaId == id && d.Carteira.UsuarioId == usuarioId);
        }

        public async Task<List<Despesa>> GetDespesasMesAsync(int usuarioId, int mes)
        {
            return await _contexto.Despesas
                        .Where(d => d.Carteira.UsuarioId == usuarioId && d.DataDespesa.Month == mes)
                        .Include(d => d.Categoria)
                        .Include(d => d.Carteira)
                        .OrderBy(d => d.DataDespesa)
                        .ToListAsync();
        }

        public async Task<List<Despesa>> GetAllDespesasPeriodoAsync(int usuarioId, DateTime inicio, DateTime fim)
        {
            return await _contexto.Despesas
                        .Where(d => d.Carteira.CarteiraId == usuarioId &&
                                DateTime.Compare(d.DataDespesa, inicio.AddDays(-1)) > 0 &&
                                DateTime.Compare(d.DataDespesa, fim.AddDays(1)) < 0
                               )
                        .AsNoTracking()
                        .ToListAsync();
        }

        public async Task<int> CreateOrEditDespesaAsync(Despesa dadosDespesa)
        {
            var despesa = await _contexto.Despesas
                          .AsNoTracking()
                          .FirstOrDefaultAsync(d => d.DespesaId == dadosDespesa.DespesaId);

            if (despesa != null)
            {
                double novoValor = dadosDespesa.Valor - despesa.Valor;
                DiminuindoSaldoCarteira(novoValor, dadosDespesa.CarteiraId);

                _contexto.Despesas.Update(dadosDespesa);
            }
            else
            {
                DiminuindoSaldoCarteira(dadosDespesa.Valor, dadosDespesa.CarteiraId);
                _contexto.Despesas.Add(dadosDespesa);
            }

            return await _contexto.SaveChangesAsync();
        }

        public async Task<int> DeleteDespesaAsync(Despesa despesa)
        {

            bool existe = _contexto.Despesas.Any(d => d.DespesaId == despesa.DespesaId);

            if (existe)
            {
                AdicionaSaldoCarteira(despesa.Valor, despesa.CarteiraId);

                _contexto.Despesas.Remove(despesa);

                return await _contexto.SaveChangesAsync();
            }
            else
            {
                return 0;
            }

        }

        private void DiminuindoSaldoCarteira(double valor, int carteiraId)
        {
            var carteira = _contexto.Carteiras
                           .AsNoTracking()
                           .FirstOrDefault(c => c.CarteiraId == carteiraId);

            carteira.Saldo -= valor;

            _contexto.Update(carteira);

            try
            {
                _contexto.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                throw e;
            }

        }

    }
}
