using ControleDeGastosV2.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ControleDeGastosV2.Contexts
{
    public class Contexto : IdentityDbContext<IdentityUser<int>, IdentityRole<int>, int> //DbContext
    {

        public Contexto(DbContextOptions<Contexto> options) : base(options)
        {

        }

        public DbSet<Carteira> Carteiras { get; set; }

        public DbSet<Categoria> Categorias { get; set; }

        public DbSet<Despesa> Despesas { get; set; }

        public DbSet<Receita> Receitas { get; set; }
    }
}
