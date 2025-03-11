using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace practicaPrestamos4.Entidades
{
    public class Loan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LoanId { get; set; }  // LoanId (bigint, primary key, autoincrement)

        [Required]
        [Display(Name = "ID del empleado")]
        public long LoanEmployeeId { get; set; }  // LoanEmployeeId (bigint, foreign key)

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Monto del préstamo")]
        public decimal LoanAmount { get; set; }  // LoanAmount (decimal(18, 2))

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Monto total a pagar")]
        public decimal LoanTotalAmountToPay { get; set; }  // LoanTotalAmountToPay (decimal(18, 2))

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Monto total a pagar con interés moroso")]
        public decimal LoanTotalAmountToPayLate { get; set; }  // LoanTotalAmountToPayLate (decimal(18, 2))

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Interés Aprobado")]

        public decimal LoanApprovedInterest { get; set; }  // LoanApprovedInterest (decimal(18, 2))

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Interés moratorio")]

        public decimal LoanLateInterest { get; set; }  // LoanLateInterest (decimal(18, 2))

        [Required]
        [Display(Name = "Tipo de pago")]
        public int LoanPaymentTypeId { get; set; }  // LoanPaymentTypeId (int, foreign key)

        [Display(Name = "Fecha del primer pago")]
        public DateTime? LoanFirstPaymentDate { get; set; }  // LoanFirstPaymentDate (datetime, nullable)

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Capital total pagado")]
        public decimal LoanTotalPaidCapital { get; set; }  // LoanTotalPaidCapital (decimal(18, 2))

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Interés total pagado")]
        public decimal LoanTotalPaidInterest { get; set; }  // LoanTotalPaidInterest (decimal(18, 2))

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Saldo Pendiente")]
        public decimal LoanBalance { get; set; }  // LoanBalance (decimal(18, 2))

        [Display(Name = "Fecha del último pago")]
        public DateTime? LoanFinalPaymentDate { get; set; }  // LoanFinalPaymentDate (datetime, nullable)

        [Display(Name = "ID del admin")]
        public int LoanUserId { get; set; }  // LoanUserId (bigint, foreign key)

        [Display(Name = "Notas")]
        public string? LoanNotes { get; set; }  // LoanNotes (nvarchar(max), nullable)

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Fecha de creación")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;  // CreatedAt (datetime, DEFAULT GETUTCDATE())

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Display(Name = "Fecha de actualización")]
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;  // UpdatedAt (datetime, DEFAULT GETUTCDATE() ON UPDATE CURRENT_TIMESTAMP)

        // Relaciones
        public Employee Employee { get; set; }  // Relación con Employee

        public PaymentType PaymentTypes { get; set; }  // Relación con PaymentTypes

        public User User { get; set; }  // Relación con User (Autor)
    }
}