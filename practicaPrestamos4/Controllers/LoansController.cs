using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using practicaPrestamos4.Data;
using practicaPrestamos4.Entidades;
using System.Linq;
using System.Security.Claims;

namespace practicaPrestamos4.Controllers
{
    [Authorize]
    public class LoansController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public LoansController(ApplicationDbContext context,
            UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Loans/Create
        public IActionResult Create(long employeeId)
        {
            // Obtener el empleado por su ID
            var employee = _context.Employees.Find(employeeId);
            if (employee == null)
            {
                return NotFound();
            }

            // Obtener la lista de tipos de pago (PaymentTypes)
            var paymentTypes = _context.PaymentTypes.ToList();

            // Pasar el empleado y los tipos de pago a la vista
            ViewBag.Employee = employee;
            ViewBag.PaymentTypes = paymentTypes;

            return View();
        }

        // POST: Loans/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Loan loan)
        {
            Console.WriteLine($"Entra a la función crear");

            if (ModelState.IsValid)
            {
                Console.WriteLine($"Empieza la función");

                // Calcular los campos restantes
                loan.LoanTotalPaidCapital = 0; // Inicialmente no se ha pagado nada
                loan.LoanTotalPaidInterest = 0; // Inicialmente no se ha pagado interés
                loan.LoanBalance = loan.LoanTotalAmountToPay; // El saldo inicial es el monto total a pagar

                // Asignar el usuario logueado como el autor del préstamo
                loan.LoanUserId = await GetCurrentUserId(); // Ahora usamos await correctamente

                // Asignar las fechas de creación y actualización
                loan.CreatedAt = DateTime.UtcNow;
                loan.UpdatedAt = DateTime.UtcNow;

                // Guardar el préstamo en la base de datos
                _context.Loans.Add(loan);
                await _context.SaveChangesAsync(); // Cambiamos a SaveChangesAsync

                return RedirectToAction("Index", "Employees"); // Redirigir a la lista de empleados
            }

            Console.WriteLine($"No funcionó");

            // Si el modelo no es válido, recargar la vista con los datos necesarios
            ViewBag.Employee = _context.Employees.Find(loan.LoanEmployeeId);
            ViewBag.PaymentTypes = _context.PaymentTypes.ToList();
            return View(loan);
        }


        // Método para obtener el ID del usuario logueado (debes implementarlo según tu sistema de autenticación)
        private async Task<int> GetCurrentUserId()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (int.TryParse(userIdString, out int userId))
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());

                if (user != null)
                {
                    return user.Id;
                }
            }

            return 0; // Si no encuentra el usuario o no está autenticado
        }
    }
}