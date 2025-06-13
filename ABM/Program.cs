using ABM.Components;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Data.SqlClient;
using ABM.DALs;
using ABM.DALs.ModelsDALs;
using ABM.Common.MMS;
using ABM.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

#region entidades de datos y negocio
builder.Services.AddSingleton<SqlConnection>(sp =>
{
    var configuration = sp.GetService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("CadenaConexion");
    return new SqlConnection(connectionString);
});

//La conexi�n se mantiene viva solo durante la duraci�n de una solicitud HTTP.
//Es m�s eficiente en aplicaciones con m�ltiples solicitudes simult�neas.
builder.Services.AddScoped<ITransaction<SqlTransaction>, SqlServerTransaction>();

builder.Services.AddScoped<PersonaModelDALs>();
builder.Services.AddScoped<UsuarioModelDALs>();
builder.Services.AddScoped<RolModelDALs>();
builder.Services.AddScoped<UsuarioRolModelDALs>();
//
builder.Services.AddScoped<PersonaService>();
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<RolService>();
#endregion

#region autentificaci�n-autorizacion
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "auth_token"; //default Cookie
        options.LoginPath = "/login";
        options.AccessDeniedPath = "/access-denied";
        options.ReturnUrlParameter = "returnurl";
        //
        options.Cookie.IsEssential = true;//algunos navegadores bloquean las cookies que no son esenciales
        options.Cookie.MaxAge = null;// TimeSpan.FromMinutes(30);
        //                             //options.IdleTimeout = TimeSpan.FromDays(30); //tiempo de inactividad
        options.Cookie.HttpOnly = true; //evita acceso de javascript
        options.Cookie.SameSite = SameSiteMode.Strict;// Lax para casos como OAuth, OpenID Connect, etc.
    });
builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddHttpContextAccessor();
#endregion


var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();
app.UseAuthentication(); // Describirlos
app.UseAuthorization();  // Describirlos

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
