using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using practicaPrestamos4.Data;
using practicaPrestamos4.Entidades;
using System.Collections.Generic;
using System.Linq;
using practicaPrestamos4.ViewModels;
using System.Security.Claims;


namespace practicaPrestamos4.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var employees = _context.Employees
                .Where(e => e.EmployeeStatus != 3)
                .Include(e => e.Loans) // Incluir los préstamos asociados al empleado
                .ToList();

            return View(employees);

        }

        public IActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Employees.Add(employee);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        public IActionResult CreateLoan(long id)
        {
            // Obtener el empleado por su ID
            var employee = _context.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }

            // Pasar el empleado a la vista
            ViewBag.Employee = employee;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DarDeBajaEmpleado(long employeeId)
        {
            // Obtener el empleado por su ID
            var employee = await _context.Employees.FindAsync(employeeId);
            if (employee == null)
            {
                return NotFound("Empleado no encontrado.");
            }

            // Cambiar el estado del empleado a 3 (dado de baja)
            employee.EmployeeStatus = 3;
            _context.Employees.Update(employee);

            // Obtener los préstamos activos del empleado
            var loans = await _context.Loans
                .Where(l => l.LoanEmployeeId == employeeId && l.LoanStatus == 1) // 1 = Préstamo activo
                .ToListAsync();

            // Recorrer cada préstamo y marcarlo como pagado
            foreach (var loan in loans)
            {
                // Marcar el préstamo como pagado
                loan.LoanTotalPaidCapital = loan.LoanTotalAmountToPay;
                loan.LoanStatus = 2; // 2 = Préstamo terminado
                _context.Loans.Update(loan);

                var lhs = await _context.LoanHistories
                    .Where(lh => lh.LoanId == loan.LoanId && lh.LoanHistoryStatus == 1)
                    .ToListAsync();

                foreach (var plh in lhs)
                {
                    plh.LoanHistoryStatus = 2;
                    plh.UpdatedAt = DateTime.UtcNow;
                    _context.LoanHistories.Update(plh); // Marcar el registro como modificado

                }

                // Crear un nuevo registro en LoanHistory
                var loanHistory = new LoanHistory
                {
                    LoanId = loan.LoanId,
                    LoanHistoryUserId = GetCurrentUserId(), // Obtener el ID del administrador logueado
                    FieldChanged = "Terminado",
                    OldValue = (loan.LoanTotalPaidCapital - loan.LoanTotalAmountToPay).ToString(), // Valor anterior (diferencia)
                    NewValue = (loan.LoanTotalPaidCapital).ToString(), // Valor nuevo (pago completo)
                    UpdatedAt = DateTime.UtcNow,
                    //LoanId1 = null,
                    LoanHistoryStatus = 2 // 2 = Préstamo terminado
                };

                _context.LoanHistories.Add(loanHistory);
            }

            // Guardar todos los cambios en la base de datos
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Employees"); // Redirigir a la lista de empleados
        }

        private int GetCurrentUserId()
        {
            // Obtener el ID del usuario desde las claims
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //Console.WriteLine($"Usuario ID desde claims: {userIdString}");

            if (string.IsNullOrEmpty(userIdString))
            {
                //Console.WriteLine("El usuario no está autenticado o no tiene un NameIdentifier.");
                return 0; // Si no está autenticado o no tiene un ID válido
            }

            if (int.TryParse(userIdString, out int userId))
            {
                // Verificar que el usuario exista en la base de datos
                var userExists = _context.Users.Any(u => u.Id == userId);
                if (!userExists)
                {
                    //Console.WriteLine($"El usuario con ID {userId} no existe en la base de datos.");
                    return 0; // Si el usuario no existe
                }

                return userId;
            }

            //Console.WriteLine($"El ID del usuario no es un número válido: {userIdString}");
            return 0; // Si el ID no es un número válido
        }




    }
}