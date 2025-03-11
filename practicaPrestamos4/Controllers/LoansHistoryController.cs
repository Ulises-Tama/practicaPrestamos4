using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using practicaPrestamos4.Data;
using practicaPrestamos4.ViewModels;
using System.Linq;
using System.Threading.Tasks;

public class LoansHistoryController : Controller
{
    private readonly ApplicationDbContext _context;

    public LoansHistoryController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: LoansHistory
    public async Task<IActionResult> Index()
    {
        var loansHistory = await _context.LoanHistories
            .Include(lh => lh.Loan) // Incluir la relación con Loan
            .ThenInclude(l => l.Employee) // Incluir la relación con Employee desde Loan
            .Select(lh => new LoanHistoryViewModel
            {
                LoanHistoryId = lh.LoanHistoryId,
                LoanId = lh.LoanId,
                EmployeeName = lh.Loan.Employee.EmployeeName, // Obtener el nombre del empleado desde Loan -> Employee
                EmployeeLastname1 = lh.Loan.Employee.EmployeeLastname1, // Obtener el apellido del empleado
                EmployeeLastname2 = lh.Loan.Employee.EmployeeLastname2, // Obtener el apellido del empleado
                FieldChanged = lh.FieldChanged,
                OldValue = lh.OldValue,
                NewValue = lh.NewValue,
                CreatedAt = lh.CreatedAt,
                UpdatedAt = lh.UpdatedAt
            })
            .ToListAsync();

        return View(loansHistory);
    }
}