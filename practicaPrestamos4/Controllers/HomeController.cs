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


public class HomeController(ILogger<HomeController> logger,
    ApplicationDbContext context) : Controller
{
    private readonly ILogger<HomeController> _logger = logger;
    private readonly ApplicationDbContext _context = context;

    [Authorize] // Solo usuarios autenticados pueden acceder a esta acción
    public IActionResult Index()
    {
        // Usar el layout para usuarios autenticados
        ViewBag.Layout = "_Layout";
        return View();
    }

    [Authorize]
    public IActionResult Privacy()
    {
        return View();
    }

    [Authorize]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [Authorize]
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
    
}
