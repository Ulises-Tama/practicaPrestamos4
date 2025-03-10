// Ignore Spelling: Prestamos practica

using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using practicaPrestamos4.Entidades;

namespace practicaPrestamos4.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSet para la entidad User
        public DbSet<User> Users { get; set; } = null!;

        // DbSet para la entidad Employee
        public DbSet<Employee> Employees { get; set; }

        // DbSet para la entidad Loan
        public DbSet<Loan> Loans { get; set; }

        // DbSet para la entidad PaymentTypess
        public DbSet<PaymentType> PaymentTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración para la entidad User
            modelBuilder.Entity<User>()
                .Property(u => u.UserStatus)
                .HasDefaultValue((byte)1);  // Valor por defecto para UserStatus

            modelBuilder.Entity<User>()
                .Property(u => u.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");  // Valor por defecto para CreatedAt

            modelBuilder.Entity<User>()
                .Property(u => u.UpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()")  // Valor por defecto para UpdatedAt
                .ValueGeneratedOnAddOrUpdate();  // Actualiza automáticamente al insertar o actualizar

            // Configuración para la entidad Employee
            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.PayrollNumber)
                .IsUnique();  // Asegurar que PayrollNumber sea único

            modelBuilder.Entity<Employee>()
                .Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");  // Valor por defecto para CreatedAt

            modelBuilder.Entity<Employee>()
                .Property(e => e.UpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()")  // Valor por defecto para UpdatedAt
                .ValueGeneratedOnAddOrUpdate();  // Actualizar automáticamente al insertar o modificar

            // Configuración para la entidad Loan
            modelBuilder.Entity<Loan>()
                .HasOne(l => l.Employee)  // Un préstamo pertenece a un empleado
                .WithMany(e => e.Loans)  // Un empleado puede tener muchos préstamos
                .HasForeignKey(l => l.LoanEmployeeId);  // Clave foránea

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Loan>()
                .HasOne(l => l.User)  // Un préstamo tiene un autor (usuario)
                .WithMany()  // Un usuario puede autorizar muchos préstamos
                .HasForeignKey(l => l.LoanUserId);  // Clave foránea

            modelBuilder.Entity<Loan>()
                .HasOne(l => l.PaymentTypes)  // Un préstamo tiene un tipo de pago
                .WithMany()  // Un tipo de pago puede estar en muchos préstamos
                .HasForeignKey(l => l.LoanPaymentTypeId);  // Clave foránea

            modelBuilder.Entity<Loan>()
                .Property(l => l.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");  // Valor por defecto para CreatedAt

            modelBuilder.Entity<Loan>()
                .Property(l => l.UpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()")  // Valor por defecto para UpdatedAt
                .ValueGeneratedOnAddOrUpdate();  // Actualizar automáticamente al insertar o modificar

            modelBuilder.Entity<PaymentType>().HasData(
                new PaymentType
                {
                    PaymentTypeId = 1,
                    ShortName = "Semanal",
                    Description = "Pago semanal",
                    CreatedAt = new DateTime(2025, 03, 09),
                    UpdatedAt = new DateTime(2025, 03, 09)

                },
                new PaymentType
                {
                    PaymentTypeId = 2,
                    ShortName = "Catorcenal",
                    Description = "Pago cada catorce días",
                    CreatedAt = new DateTime(2025, 03, 09),
                    UpdatedAt = new DateTime(2025, 03, 09)
                },
                new PaymentType
                {
                    PaymentTypeId = 3,
                    ShortName = "Quincenal",
                    Description = "Pago cada quince días",
                    CreatedAt = new DateTime(2025, 03, 09),
                    UpdatedAt = new DateTime(2025, 03, 09)
                },
                new PaymentType
                {
                    PaymentTypeId = 4,
                    ShortName = "Mensual",
                    Description = "Pago mensual",
                    CreatedAt = new DateTime(2025, 03, 09),
                    UpdatedAt = new DateTime(2025, 03, 09)
                },
                new PaymentType
                {
                    PaymentTypeId = 5,
                    ShortName = "Al Final",
                    Description = "Pago único al final del préstamo",
                    CreatedAt = new DateTime(2025, 03, 09),
                    UpdatedAt = new DateTime(2025, 03, 09)
                }
            );

            modelBuilder.Entity<LoanHistory>()
                .HasOne(lh => lh.Loan)
                .WithMany()
                .HasForeignKey(lh => lh.LoanId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LoanHistory>()
                .HasOne(lh => lh.User)
                .WithMany()
                .HasForeignKey(lh => lh.LoanHistoryUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}