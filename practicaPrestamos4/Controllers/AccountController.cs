using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using practicaPrestamos4.Entidades;
using System.Security.Claims;
using System.Threading.Tasks;

namespace practicaPrestamos4.Controllers
{
    public class AccountController : Controller
    {
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
            if (IsValidUser(email, password))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Name, email)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
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
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        private bool IsValidUser(string email, string password)
        {
            using (var connection = new SqlConnection("YourConnectionString"))
            {
                var query = "SELECT Password FROM Users WHERE Email = @Email";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Email", email);

                connection.Open();
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    var storedPassword = reader["Password"].ToString();
                    return password == storedPassword; // Comparación directa de contraseñas
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
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(name))
            {
                ModelState.AddModelError("", "El correo electrónico, la contraseña y el nombre son requeridos.");
                ViewBag.Layout = "_LayoutUnauthenticated";  // Usar el layout para usuarios no autenticados
                return View();
            }

            if (await IsEmailExists(email))
            {
                ModelState.AddModelError("", "El correo electrónico ya está registrado.");
                ViewBag.Layout = "_LayoutUnauthenticated";  // Usar el layout para usuarios no autenticados
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

            using (var connection = new SqlConnection("YourConnectionString"))
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

            return RedirectToAction("Login");
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
            using (var connection = new SqlConnection("YourConnectionString"))
            {
                var query = "SELECT COUNT(*) FROM Users WHERE Email = @Email";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Email", email);

                connection.Open();
                var count = (int)await command.ExecuteScalarAsync();
                return count > 0;
            }
        }
    }
}