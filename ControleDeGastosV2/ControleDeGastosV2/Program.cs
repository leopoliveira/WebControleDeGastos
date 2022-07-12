using ControleDeGastosV2.Contexts;
using ControleDeGastosV2.Repositories;
using ControleDeGastosV2.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Servi�o de pagina��o pacote: ReflectionIT.Mvc.Paging
builder.Services.AddPaging(options =>
{
    options.ViewName = "Bootstrap4";
    options.PageParameterName = "pageindex";

});

//String de Conex�o
string _dataBaseConnectionString = builder.Configuration.GetConnectionString("DevConnection");

//Servi�o de Inje��o de Depend�ncia do Contexto do Banco de Dados para ser usado na aplica��o
builder.Services.AddDbContext<Contexto>(options => options.UseSqlServer(_dataBaseConnectionString));

// Configurando o Identity para Autoriza��o e Autentica��o
builder.Services.AddIdentity<IdentityUser<int>, IdentityRole<int>>()
                .AddEntityFrameworkStores<Contexto>()
                .AddDefaultTokenProviders();

builder.Services.AddAuthorization();

//Servi�os de Inje��o de Depend�ncia dos Repositories
    //Scoped para manter a inst�ncia do Objeto durante toda a requisi��o, ou request.
    //Poderia ser Singleton, caso eu fosse manter a inst�ncia por toda a vida �til da aplica��o.
    //Ou Transient, onde eu crio a inst�ncia a cada solicita��o do servi�o.
builder.Services.AddScoped<ICarteiraRepository, CarteiraRepository>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IReceitaDespesaRepository, ReceitaDespesaRepository>();
builder.Services.AddScoped<IResumoRepository, ResumoRepository>();

// Configura��es de cria��o de usu�rio
builder.Services.Configure<IdentityOptions>(options =>
{
    // Configura��es de Senha
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

    // Configura��es de Usu�rio
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
    // Configura��o Login/Logout
    options.LoginPath = "/Usuario/Index";
    options.LogoutPath = "/Usuario/Index";
    //options.AccessDeniedPath = "/Usuario/AcessoRestrito";
    options.ReturnUrlParameter = "returnUrl";

    // Configura��es do Cookie
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
    options.Cookie.Name = "ControleDeGastosApp";

});

builder.Services.ConfigureApplicationCookie(options =>
{
    // Configura��es do Cookie
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
