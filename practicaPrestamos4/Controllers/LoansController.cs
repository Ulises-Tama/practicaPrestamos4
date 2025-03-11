using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using practicaPrestamos4.Data;
using practicaPrestamos4.Entidades;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace practicaPrestamos4.Controllers
{
    [Authorize]
    public class LoansController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoansController(ApplicationDbContext context)
        {
            _context = context;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LoanId,LoanEmployeeId,LoanAmount,LoanTotalAmountToPay,LoanTotalAmountToPayLate,LoanApprovedInterest,LoanLateInterest,LoanPaymentTypeId,LoanFirstPaymentDate,LoanTotalPaidCapital,LoanTotalPaidInterest,LoanBalance,LoanFinalPaymentDate,LoanUserId,LoanNotes,CreatedAt,UpdatedAt")] Loan loan)
        {
            Console.WriteLine("Checa modelo");

            if (loan == null)
            {
                Console.WriteLine("El modelo Loan es nulo");
            }
            else
            {
                Console.WriteLine($"LoanEmployeeId: {loan.LoanEmployeeId}");
                Console.WriteLine($"LoanAmount: {loan.LoanAmount}");
                Console.WriteLine($"LoanPaymentTypeId: {loan.LoanPaymentTypeId}");
                Console.WriteLine($"LoanFirstPaymentDate: {loan.LoanFirstPaymentDate}");
                Console.WriteLine($"LoanFinalPaymentDate: {loan.LoanFinalPaymentDate}");
                Console.WriteLine($"LoanApprovedInterest: {loan.LoanApprovedInterest}");
                Console.WriteLine($"LoanLateInterest: {loan.LoanLateInterest}");
                Console.WriteLine($"LoanNotes: {loan.LoanNotes}");
            }

            if (!ModelState.IsValid)
            {
                foreach (var key in ModelState.Keys)
                {
                    var state = ModelState[key];
                    if (state.Errors.Any())
                    {
                        Console.WriteLine($"Error en {key}: {state.Errors.First().ErrorMessage}");
                    }
                }
            }

            if (ModelState.IsValid)
            {
                Console.WriteLine("Entró al if");

                // Calcular los campos restantes
                loan.LoanTotalPaidCapital = 0; // Inicialmente no se ha pagado nada
                loan.LoanTotalPaidInterest = 0; // Inicialmente no se ha pagado interés

                // Calcular el monto total a pagar sin intereses moratorios
                loan.LoanTotalAmountToPay = loan.LoanAmount + (loan.LoanAmount * (loan.LoanApprovedInterest / 100));

                // Calcular el monto total a pagar con intereses moratorios
                loan.LoanTotalAmountToPayLate = loan.LoanAmount + (loan.LoanAmount * (loan.LoanLateInterest / 100));

                // Asignar el saldo inicial
                loan.LoanBalance = loan.LoanTotalAmountToPayLate; // El saldo inicial es el monto total a pagar con intereses moratorios

                // Asignar el usuario logueado como el autor del préstamo
                loan.LoanUserId = GetCurrentUserId();

                // Asignar las fechas de creación y actualización
                loan.CreatedAt = DateTime.UtcNow;
                loan.UpdatedAt = DateTime.UtcNow;

                // Guardar el préstamo en la base de datos
                _context.Loans.Add(loan);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }

            // Si el modelo no es válido, recargar la vista con los datos necesarios
            ViewBag.Employee = _context.Employees.Find(loan?.LoanEmployeeId);
            ViewBag.PaymentTypes = _context.PaymentTypes.ToList();
            return View(loan);
        }

        // Método para obtener el ID del usuario logueado
        private int GetCurrentUserId()
        {
            // Obtener el ID del usuario desde las claims
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Console.WriteLine($"usuario: {userIdString}");

            if (int.TryParse(userIdString, out int userId))
            {
                return userId;
            }

            return 0; // Si no encuentra el usuario o no está autenticado
        }
    }
}