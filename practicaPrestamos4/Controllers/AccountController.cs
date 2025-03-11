using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using practicaPrestamos4.Entidades;
using System.Security.Claims;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace practicaPrestamos4.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;

        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            ViewBag.Layout = "_LayoutUnauthenticated";  // Usar el layout para usuarios no autenticados
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password, string returnUrl = null)
        {
            Console.WriteLine($" campos del login son: {email}, {password}, {returnUrl}");
            if (IsValidUser(email, password, out var userId)) // Modifica IsValidUser para devolver el ID del usuario
            {
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Name, email),
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()) // Agrega el ID del usuario como Claim
        };

                var claimsIdentity = new ClaimsIdentity(claims, "CookieAuth"); // Usa el mismo esquema
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true
                };

                await HttpContext.SignInAsync(
                    "CookieAuth", // Usa el mismo esquema
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Correo electrónico o contraseña incorrectos.");
            ViewBag.Layout = "_LayoutUnauthenticated";  // Usar el layout para usuarios no autenticados
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuth"); // Usa el mismo esquema que configuraste
            return RedirectToAction("Login");
        }

        private bool IsValidUser(string email, string password, out int userId)
        {
            userId = 0; // Inicializa el ID del usuario

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var query = "SELECT Id, Password FROM Users WHERE Email = @Email";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Email", email);

                connection.Open();
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    var storedPassword = reader["Password"].ToString();
                    if (password == storedPassword) // Comparación directa de contraseñas
                    {
                        userId = Convert.ToInt32(reader["Id"]); // Obtén el ID del usuario
                        return true;
                    }
                }
                return false;
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            ViewBag.Layout = "_LayoutUnauthenticated";  // Usar el layout para usuarios no autenticados
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(string email, string password, string name)
        {
            Console.WriteLine($"Entra a la función crear con: {email}, {password}, {name}");

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(name))
            {
                ModelState.AddModelError("", "El correo electrónico, la contraseña y el nombre son requeridos.");
                ViewBag.Layout = "_LayoutUnauthenticated";
                return View();
            }

            if (await IsEmailExists(email))
            {
                ModelState.AddModelError("", "El correo electrónico ya está registrado.");
                ViewBag.Layout = "_LayoutUnauthenticated";
                return View();
            }

            var user = new User
            {
                Email = email,
                Password = password, // Almacenar la contraseña en texto plano
                Name = name,
                NormalizedEmail = email.ToUpper(),
                UserStatus = 2, // Estado del usuario (2 = Activo)
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            Console.WriteLine($"User con: {user}");

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var query = @"
                    INSERT INTO Users (Email, Password, Name, NormalizedEmail, UserStatus, CreatedAt, UpdatedAt)
                    VALUES (@Email, @Password, @Name, @NormalizedEmail, @UserStatus, @CreatedAt, @UpdatedAt)";

                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@Password", user.Password);
                command.Parameters.AddWithValue("@Name", user.Name);
                command.Parameters.AddWithValue("@NormalizedEmail", user.NormalizedEmail);
                command.Parameters.AddWithValue("@UserStatus", user.UserStatus);
                command.Parameters.AddWithValue("@CreatedAt", user.CreatedAt);
                command.Parameters.AddWithValue("@UpdatedAt", user.UpdatedAt);

                connection.Open();
                await command.ExecuteNonQueryAsync();
            }

            // Autenticar al usuario después del registro
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name)
            };

            var claimsIdentity = new ClaimsIdentity(claims, "CookieAuth");
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true
            };

            await HttpContext.SignInAsync(
                "CookieAuth",
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            ViewBag.Layout = "_LayoutUnauthenticated";  // Usar el layout para usuarios no autenticados
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword()
        {
            ViewBag.Layout = "_LayoutUnauthenticated";  // Usar el layout para usuarios no autenticados
            return View();
        }

        private async Task<bool> IsEmailExists(string email)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var query = "SELECT COUNT(1) FROM Users WHERE Email = @Email";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Email", email);

                connection.Open();
                var count = (int)await command.ExecuteScalarAsync();
                return count > 0;
            }
        }

    }
}