using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace practicaPrestamos4.Entidades
{
    public class PaymentType
    {
        [Key]
        public int PaymentTypeId { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("ShortName")] // Mapear a la columna "ShortName" en la base de datos
        [Display(Name = "Nombre")]

        public string ShortName { get; set; }

        [MaxLength(200)]
        [Column("Description")] // Mapear a la columna "Description" en la base de datos
        [Display(Name = "Descripción")]
        public string? Description { get; set; }

        [Display(Name = "Fecha de creación")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Display(Name = "Fecha de actualización")]
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}