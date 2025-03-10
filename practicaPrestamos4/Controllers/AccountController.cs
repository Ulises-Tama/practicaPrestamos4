using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using practicaPrestamos4.Entidades;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace practicaPrestamos4.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public AccountController(SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string email, string password, string returnUrl = null)
        {
            Console.WriteLine("Entró a la función de login.");
            Console.WriteLine($"{email} con {password}");

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "El correo electrónico y la contraseña son requeridos.");
                return View();
            }

            // Buscar el usuario por email
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                ModelState.AddModelError("", "El correo electrónico no está registrado.");
                return View();
            }

            // Intentar iniciar sesión
            var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
            Console.WriteLine($"{result}");
            if (result.Succeeded)
            {
                Console.WriteLine("Login exitoso.");
                return RedirectToLocal(returnUrl);
            }
            Console.WriteLine("Login NO exitoso.");

            ModelState.AddModelError("", "Correo o contraseña incorrectos.");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(string email, string password)
        {
            var user = new User
            {
                UserName = email,
                Email = email
            };

            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                return RedirectToAction("Login");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View();
        }
        public IActionResult ForgotPassword()
        {
            // Usar el layout para usuarios no autenticados
            ViewBag.Layout = "_LayoutUnauthenticated";
            return View();
        }
        public IActionResult ResetPassword()
        {
            // Usar el layout para usuarios no autenticados
            ViewBag.Layout = "_LayoutUnauthenticated";
            return View();
        }


    }
}
