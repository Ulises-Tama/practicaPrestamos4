using Microsoft.AspNetCore.Mvc;
using practicaPrestamos4.Data;
using practicaPrestamos4.Entidades;
using System.Collections.Generic;
using System.Linq;

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
            // Filtrar empleados donde EmployeeStatus no sea igual a 3
            var employees = _context.Employees
                .Where(e => e.EmployeeStatus != 3)
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
    }
}