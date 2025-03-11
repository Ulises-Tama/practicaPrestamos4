using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace practicaPrestamos4.Entidades
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long EmployeeId { get; set; }  // Employee_id (bigint, primary key, autoincrement)

        [Required(ErrorMessage = "El número de nómina es obligatorio.")]
        [MaxLength(25)]
        [Display(Name = "Número de nómina")]
        public string PayrollNumber { get; set; }  // PayrollNumber (varchar(25), NOT NULL, UNIQUE)

        [Required]
        [MaxLength(100)]
        [Display(Name = "Nombre")]
        public string EmployeeName { get; set; }  // Employee_name (varchar(100), NOT NULL)

        [MaxLength(100)]
        [Display(Name = "Primer apellido")]
        public string EmployeeLastname1 { get; set; }  // Employee_lastname1 (varchar(100), NULL)

        [MaxLength(100)]
        [Display(Name = "Segundo apellido")]
        public string EmployeeLastname2 { get; set; }  // Employee_lastname2 (varchar(100), NULL)

        [Required]
        [Display(Name = "Estatus")]
        public byte EmployeeStatus { get; set; } = 1;  // Employee_status (tinyint, NOT NULL, DEFAULT 1)

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Fecha de creación")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;  // CreatedAt (datetime, DEFAULT GETUTCDATE())

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Display(Name = "Fecha de actualización")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;  // UpdatedAt (datetime, DEFAULT GETUTCDATE() ON UPDATE CURRENT_TIMESTAMP)


        // Relación con Loan
        public ICollection<Loan> Loans { get; set; } = new List<Loan>();

    }
}