namespace practicaPrestamos4.ViewModels
{
    public class LoanHistoryViewModel
    {
        public int LoanHistoryId { get; set; }
        public long LoanId { get; set; }
        public string EmployeeName { get; set; }    
        public string EmployeeLastname1 { get; set; } 
        public string EmployeeLastname2 { get; set; } 
        public string FieldChanged { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
