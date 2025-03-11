﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using practicaPrestamos4.Data;
using practicaPrestamos4.Entidades;
using System.Collections.Generic;
using System.Linq;
using practicaPrestamos4.ViewModels; 


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
            //var employees = _context.Employees
            //    .Where(e => e.EmployeeStatus != 3) // Solo empleados activos
            //    .Include(e => e.Loans) // Incluir los préstamos
            //    .ToList() // Convertir a lista (sin await)
            //    .Select(e => new EmployeeWithLoansViewModel
            //    {
            //        EmployeeId = e.EmployeeId,
            //        PayrollNumber = e.PayrollNumber,
            //        EmployeeName = e.EmployeeName,
            //        EmployeeLastname1 = e.EmployeeLastname1,
            //        EmployeeLastname2 = e.EmployeeLastname2,
            //        EmployeeStatus = e.EmployeeStatus,
            //        ActiveLoans = e.Loans
            //            .Where(l => l.LoanBalance > 0 && l.LoanBalance < l.LoanTotalAmountToPay)
            //            .Select(l => new LoanViewModel
            //            {
            //                LoanId = l.LoanId,
            //                LoanTotalAmountToPay = l.LoanTotalAmountToPay,
            //                LoanTotalAmountToPayLate = l.LoanTotalAmountToPayLate,
            //                LoanBalance = l.LoanBalance
            //            })
            //            .ToList()
            //    })
            //    .ToList();

            var employees = _context.Employees
                .Where(e => e.EmployeeStatus != 3)
                .Include(e => e.Loans) // Incluir los préstamos asociados al empleado
                .ToList();

            //var employees = _context.Employees
            //    .Where(e => e.EmployeeStatus != 3) // Filtrar empleados con estado menor a 3
            //    .Select(e => new
            //    {
            //        Employee = e,
            //        ActiveLoans = e.Loans.Where(l => l.LoanBalance < l.LoanTotalAmountToPay).ToList() // Filtrar préstamos no pagados
            //    })
            //    .Where(e => e.ActiveLoans.Any()) // Solo incluir empleados con préstamos activos
            //    .Select(e => e.Employee)
            //    .ToList();

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