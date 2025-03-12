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

        public async Task<IActionResult> Index()
        {
            // Obtener la lista de préstamos desde la base de datos
            var loans = await _context.Loans
                .Where(l => l.LoanStatus != 2) // Filtro antes de los Include
                .Include(l => l.Employee)      // Incluir la relación con Employee
                .Include(l => l.PaymentTypes)  // Incluir la relación con PaymentTypes
                .Include(l => l.User)          // Incluir la relación con User
                .ToListAsync();                // Ejecutar la consulta y obtener los resultados

            return View(loans); // Pasar la lista de préstamos a la vista
        }

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
        public async Task<IActionResult> Create(
    [Bind("LoanId,LoanEmployeeId,LoanAmount,LoanTotalAmountToPay,LoanTotalAmountToPayLate,LoanApprovedInterest,LoanLateInterest,LoanPaymentTypeId,LoanFirstPaymentDate,LoanTotalPaidCapital,LoanTotalPaidInterest,LoanBalance,LoanFinalPaymentDate,LoanUserId,LoanNotes,CreatedAt,UpdatedAt")]
    Loan loan)
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

                // Si es una solicitud AJAX, devolver un JSON con los errores
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
                }

                // Si no es AJAX, recargar la vista con los datos necesarios
                ViewBag.Employee = _context.Employees.Find(loan?.LoanEmployeeId);
                ViewBag.PaymentTypes = _context.PaymentTypes.ToList();
                return View(loan);
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

                // Actualizar el EmployeeStatus del empleado a 2 (Préstamo activo)
                var employee = await _context.Employees.FindAsync(loan.LoanEmployeeId);
                if (employee != null)
                {
                    employee.EmployeeStatus = 2; // 2 = Préstamo activo
                    _context.Employees.Update(employee);
                    await _context.SaveChangesAsync();
                }

                // Si es una solicitud AJAX, devolver un JSON con éxito
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true });
                }

                // Si no es AJAX, redirigir a la vista Index
                return RedirectToAction("Index", "Loans");
            }

            // Si el modelo no es válido y no es AJAX, recargar la vista con los datos necesarios
            ViewBag.Employee = _context.Employees.Find(loan?.LoanEmployeeId);
            ViewBag.PaymentTypes = _context.PaymentTypes.ToList();
            return View(loan);
        }

        // Método para obtener el ID del usuario logueado
        private int GetCurrentUserId()
        {
            // Obtener el ID del usuario desde las claims
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Console.WriteLine($"Usuario ID desde claims: {userIdString}");

            if (string.IsNullOrEmpty(userIdString))
            {
                Console.WriteLine("El usuario no está autenticado o no tiene un NameIdentifier.");
                return 0; // Si no está autenticado o no tiene un ID válido
            }

            if (int.TryParse(userIdString, out int userId))
            {
                // Verificar que el usuario exista en la base de datos
                var userExists = _context.Users.Any(u => u.Id == userId);
                if (!userExists)
                {
                    Console.WriteLine($"El usuario con ID {userId} no existe en la base de datos.");
                    return 0; // Si el usuario no existe
                }

                return userId;
            }

            Console.WriteLine($"El ID del usuario no es un número válido: {userIdString}");
            return 0; // Si el ID no es un número válido
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Pay(long loanId, decimal amount)
        {
            if (amount <= 0)
            {
                ModelState.AddModelError("", "La cantidad a abonar debe ser mayor que cero.");
                return RedirectToAction("Index");
            }

            // Obtener el préstamo
            var loan = await _context.Loans
                .Include(l => l.LoanHistories) // Incluir el historial si es necesario
                .FirstOrDefaultAsync(l => l.LoanId == loanId);

            if (loan == null)
            {
                return NotFound();
            }

            // Verificar que el monto a abonar no exceda el saldo pendiente
            decimal saldoPendiente = loan.LoanTotalAmountToPay - loan.LoanTotalPaidCapital;
            if (amount > saldoPendiente)
            {
                ModelState.AddModelError("", "La cantidad a abonar excede el saldo pendiente.");
                return RedirectToAction("Index");
            }

            // Guardar el valor antiguo del capital pagado
            decimal oldCapitalPaid = loan.LoanTotalPaidCapital;

            // Actualizar el préstamo
            loan.LoanTotalPaidCapital += amount;
            loan.LoanBalance = loan.LoanTotalAmountToPayLate - loan.LoanTotalPaidCapital;
            loan.UpdatedAt = DateTime.UtcNow;

            // Crear un registro en LoanHistory
            var loanHistory = new LoanHistory
            {
                LoanId = loanId,
                LoanHistoryUserId = GetCurrentUserId(), // Obtener el ID del usuario actual
                FieldChanged = "LoanTotalPaidCapital", // Campo modificado
                OldValue = oldCapitalPaid.ToString("N2"), // Valor antiguo
                NewValue = loan.LoanTotalPaidCapital.ToString("N2"), // Valor nuevo
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Agregar el historial al contexto
            _context.LoanHistories.Add(loanHistory);

            // Guardar los cambios en la base de datos
            await _context.SaveChangesAsync();

            // Verificar si el préstamo está completamente liquidado
            if (loan.LoanTotalPaidCapital >= loan.LoanTotalAmountToPay)
            {
                // Actualizar el EmployeeStatus del empleado a 1 (Préstamo liquidado)
                var employee = await _context.Employees.FindAsync(loan.LoanEmployeeId);
                if (employee != null)
                {
                    employee.EmployeeStatus = 1; // 1 = Préstamo liquidado
                    _context.Employees.Update(employee);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployeesAndPaymentTypes()
        {
            var employees = await _context.Employees
                .Select(e => new
                {
                    e.EmployeeId,
                    FullName = $"{e.EmployeeName} {e.EmployeeLastname1} {e.EmployeeLastname2}"
                })
                .ToListAsync();

            var paymentTypes = await _context.PaymentTypes
                .Select(p => new
                {
                    p.PaymentTypeId,
                    p.ShortName,
                    p.Description
                })
                .ToListAsync();

            return Json(new { employees, paymentTypes });
        }

        [HttpGet]
        public async Task<IActionResult> SearchEmployees(string searchTerm)
        {
            var employees = await _context.Employees
                .Where(e => e.EmployeeName.Contains(searchTerm)) // Filtrar por nombre
                .Select(e => new
                {
                    id = e.EmployeeId,
                    text = $"{e.EmployeeName} {e.EmployeeLastname1} {e.EmployeeLastname2}"
                })
                .ToListAsync();

            return Json(new { employees });
        }

    }
}