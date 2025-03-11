using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace practicaPrestamos4.Entidades
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }  // Llave primaria autoincrementable

        [Required]
        [MaxLength(256)]
        public string Name { get; set; } = string.Empty;  // Nombre del usuario

        [Required]
        [EmailAddress]
        [MaxLength(256)]
        public string Email { get; set; } = string.Empty;  // Correo electrónico

        [MaxLength(256)]
        public string NormalizedEmail { get; set; } = string.Empty;  // Correo electrónico normalizado (en mayúsculas)

        [Required]
        public string Password { get; set; } = string.Empty;  // Contraseña

        [Required]
        public byte UserStatus { get; set; } = 2;  // User status (1 = SuperAdmin, 2 = Activo, 3 = Inactivo)

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;  // Fecha de creación

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;  // Fecha de actualización
    }
}