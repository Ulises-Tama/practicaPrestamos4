using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace practicaPrestamos4.Entidades
{
    public class LoanHistory
    {
        [Key]
        public int LoanHistoryId { get; set; }

        // FK al préstamo que fue modificado
        [Required]
        [ForeignKey("Loan")]
        public long LoanId { get; set; }
        public Loan Loan { get; set; }

        // FK al usuario que hizo el cambio
        [Required]
        [ForeignKey("User")]
        public int LoanHistoryUserId { get; set; }
        public User User { get; set; }

        // Campo que fue modificado
        [Required]
        [MaxLength(100)]
        public string FieldChanged { get; set; }

        // Valor antes del cambio
        [MaxLength(500)]
        public string? OldValue { get; set; }

        // Valor después del cambio
        [MaxLength(500)]
        public string? NewValue { get; set; }

        [Required]
        [Display(Name = "Estatus")]
        public byte LoanHistoryStatus { get; set; } = 1;  // Employee_status (tinyint, NOT NULL, DEFAULT 1 = activo, 2 = pagado)


        // Estilo Laravel: CreatedAt y UpdatedAt
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }


    }
}
