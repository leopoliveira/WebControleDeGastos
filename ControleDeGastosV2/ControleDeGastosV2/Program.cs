using ControleDeGastosV2.Contexts;
using ControleDeGastosV2.Repositories;
using ControleDeGastosV2.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Serviço de paginação pacote: ReflectionIT.Mvc.Paging
builder.Services.AddPaging(options =>
{
    options.ViewName = "Bootstrap4";
    options.PageParameterName = "pageindex";

});

//String de Conexão
string _dataBaseConnectionString = builder.Configuration.GetConnectionString("DevConnection");

//Serviço de Injeção de Dependência do Contexto do Banco de Dados para ser usado na aplicação
builder.Services.AddDbContext<Contexto>(options => options.UseSqlServer(_dataBaseConnectionString));

// Configurando o Identity para Autorização e Autenticação
builder.Services.AddIdentity<IdentityUser<int>, IdentityRole<int>>()
                .AddEntityFrameworkStores<Contexto>()
                .AddDefaultTokenProviders();

builder.Services.AddAuthorization();

//Serviços de Injeção de Dependência dos Repositories
    //Scoped para manter a instância do Objeto durante toda a requisição, ou request.
    //Poderia ser Singleton, caso eu fosse manter a instância por toda a vida útil da aplicação.
    //Ou Transient, onde eu crio a instância a cada solicitação do serviço.
builder.Services.AddScoped<ICarteiraRepository, CarteiraRepository>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IReceitaDespesaRepository, ReceitaDespesaRepository>();
builder.Services.AddScoped<IResumoRepository, ResumoRepository>();

// Configurações de criação de usuário
builder.Services.Configure<IdentityOptions>(options =>
{
    // Configurações de Senha
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    //options.Password.RequiredUniqueChars = 1;
    //options.Password.RequiredLength = 6;

    // Bloqueio de Conta
    options.Lockout.AllowedForNewUsers = true;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
    options.Lockout.MaxFailedAccessAttempts = 3;

    // Configurações de Usuário
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;

    // Acesso
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;

});

builder.Services.ConfigureApplicationCookie(options =>
{
    // Configuração Login/Logout
    options.LoginPath = "/Usuario/Index";
    options.LogoutPath = "/Usuario/Index";
    //options.AccessDeniedPath = "/Usuario/AcessoRestrito";
    options.ReturnUrlParameter = "returnUrl";

    // Configurações do Cookie
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
    options.Cookie.Name = "ControleDeGastosApp";

});

builder.Services.ConfigureApplicationCookie(options =>
{
    // Configurações do Cookie
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(10);

    //options.LoginPath = "";
    //options.AccessDeniedPath = "";
    //options.SlidingExpiration = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Usuario}/{action=Index}/{id?}");

app.Run();
