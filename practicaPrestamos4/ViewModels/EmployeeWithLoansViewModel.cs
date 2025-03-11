namespace practicaPrestamos4.ViewModels
{
    public class EmployeeWithLoansViewModel
    {
        public long EmployeeId { get; set; }
        public string PayrollNumber { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeLastname1 { get; set; }
        public string EmployeeLastname2 { get; set; }
        public byte EmployeeStatus { get; set; }
        public List<LoanViewModel> ActiveLoans { get; set; } = new List<LoanViewModel>();
    }

    public class LoanViewModel
    {
        public long LoanId { get; set; }
        public decimal LoanTotalAmountToPay { get; set; }
        public decimal LoanTotalAmountToPayLate { get; set; }
        public decimal LoanBalance { get; set; }
    }
}
