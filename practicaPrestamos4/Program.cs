using Microsoft.EntityFrameworkCore;
using practicaPrestamos4.Data;

var builder = WebApplication.CreateBuilder(args);

// Configuraci�n de la base de datos
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configuraci�n de autenticaci�n basada en cookies
builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", options =>
    {
        options.LoginPath = "/Account/Login"; // Ruta para el login
        options.LogoutPath = "/Account/Logout"; // Ruta para el logout
        options.AccessDeniedPath = "/Account/AccessDenied"; // Ruta para acceso denegado
    });

// Configuraci�n de autorizaci�n
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AuthenticatedUser", policy =>
    {
        policy.RequireAuthenticatedUser(); // Solo requiere que el usuario est� autenticado
    });
});


var app = builder.Build();

// Configuraci�n del pipeline de solicitudes HTTP.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Aseg�rate de llamar a UseAuthentication antes de UseAuthorization
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();