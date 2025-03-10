using System.Diagnostics;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using practicaPrestamos4.Data;
using practicaPrestamos4.Entidades;
using practicaPrestamos4.Models;
using Microsoft.AspNetCore.Authorization;

namespace practicaPrestamos4.Controllers;
[Authorize(Policy = "AuthenticatedUser")]  // Proteger todas las acciones del controlador


public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, 
        ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    [Authorize] // Solo usuarios autenticados pueden acceder a esta acción
    public IActionResult Index()
    {
        // Usar el layout para usuarios autenticados
        ViewBag.Layout = "_Layout";
        return View();
    }


    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    // Acción para mostrar la vista de login
    [AllowAnonymous]
    public IActionResult Login(string returnUrl = null)
    {
        // Usar el layout para usuarios no autenticados
        ViewBag.Layout = "_LayoutUnauthenticated";

        // Guardar la URL de retorno en ViewData para usarla en el formulario
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }


    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Login(string email, string password, string returnUrl = null)
    {
        Console.WriteLine("Entró a la función de login.");

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            ModelState.AddModelError("", "El correo electrónico y la contraseña son requeridos.");
            return View();
        }

        try
        {
            // Buscar el usuario en la base de datos por correo electrónico
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                ModelState.AddModelError("", "El correo electrónico no está registrado.");
                return View();
            }

            // Verificar la contraseña (aquí deberías usar un sistema de hashing)
            if (user.Password != password)
            {
                ModelState.AddModelError("", "La contraseña es incorrecta.");
                return View();
            }

            // Crear una identidad para el usuario
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Name)
        };

            var identity = new ClaimsIdentity(claims, "CookieAuth");
            var principal = new ClaimsPrincipal(identity);

            // Autenticar al usuario usando cookies
            await HttpContext.SignInAsync("CookieAuth", principal);

            // Redirigir al usuario a la URL de retorno o a la vista Index
            Console.WriteLine("Login exitoso.");
            Console.WriteLine($"{returnUrl}");
            return RedirectToLocal(returnUrl);
        }
        catch (Exception ex)
        {
            // Registrar un error si algo sale mal
            _logger.LogError(ex, "Error al intentar iniciar sesión: {Email}", email);
            ModelState.AddModelError("", "Ocurrió un error al intentar iniciar sesión. Inténtalo de nuevo.");
            return View();
        }
    }

    // Método para redirigir al usuario a la URL de retorno o a la página principal
    private IActionResult RedirectToLocal(string returnUrl)
    {
        if (Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }
        else
        {
            return RedirectToAction("Index", "Home"); // Redirigir a la página principal
        }
    }

    public IActionResult Register()
    {
        // Usar el layout para usuarios no autenticados
        ViewBag.Layout = "_LayoutUnauthenticated";
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Register(User user)
    {
        Console.WriteLine("Entró a la función.");

        // Asignar el valor de NormalizedEmail antes de validar el modelo
        user.NormalizedEmail = user.Email?.ToUpper() ?? string.Empty;
        Console.WriteLine($"correo normalizado: {user.NormalizedEmail}");


        if (!ModelState.IsValid)  // Cambiado temporalmente para depurar
        {
            // Mostrar los errores de validación en la consola
            foreach (var key in ModelState.Keys)
            {
                var state = ModelState[key];
                if (state.Errors.Any())
                {
                    Console.WriteLine($"Error en {key}: {state.Errors.First().ErrorMessage}");
                }
            }

            return View(user);
        }

        Console.WriteLine("Empieza a ejecutar la función.");

        try
        {
            // Verificar si el correo electrónico ya existe en la base de datos
            bool emailExists = await _context.Users.AnyAsync(u => u.Email == user.Email);

            if (emailExists)
            {
                // Mostrar un mensaje de error si el correo ya está registrado
                ModelState.AddModelError("Email", "El correo electrónico ya está registrado.");
                return View(user);
            }

            // Agregar el usuario a la base de datos
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Registrar un mensaje de información
            _logger.LogInformation("Nuevo usuario registrado: {Email}", user.Email);

            // Redirigir al usuario a la vista Index
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            // Registrar un error si algo sale mal
            _logger.LogError(ex, "Error al registrar el usuario: {Email}", user.Email);
            ModelState.AddModelError("", "Ocurrió un error al registrar el usuario. Inténtalo de nuevo.");
            return View(user);
        }
    }
    


}
