﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using practicaPrestamos4.Data;

#nullable disable

namespace practicaPrestamos4.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250314155734_ChangedFieldChangedLoanHistory")]
    partial class ChangedFieldChangedLoanHistory
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("practicaPrestamos4.Entidades.Employee", b =>
                {
                    b.Property<long>("EmployeeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("EmployeeId"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("EmployeeLastname1")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("EmployeeLastname2")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("EmployeeName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<byte>("EmployeeStatus")
                        .HasColumnType("tinyint");

                    b.Property<string>("PayrollNumber")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.HasKey("EmployeeId");

                    b.HasIndex("PayrollNumber")
                        .IsUnique();

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("practicaPrestamos4.Entidades.Loan", b =>
                {
                    b.Property<long>("LoanId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("LoanId"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<decimal>("LoanAmount")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<decimal>("LoanApprovedInterest")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<decimal>("LoanBalance")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<long>("LoanEmployeeId")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("LoanFinalPaymentDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("LoanFirstPaymentDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("LoanLateInterest")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<string>("LoanNotes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("LoanPaymentTypeId")
                        .HasColumnType("int");

                    b.Property<byte>("LoanStatus")
                        .HasColumnType("tinyint");

                    b.Property<decimal>("LoanTotalAmountToPay")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<decimal>("LoanTotalAmountToPayLate")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<decimal>("LoanTotalPaidCapital")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<decimal>("LoanTotalPaidInterest")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<int>("LoanUserId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.HasKey("LoanId");

                    b.HasIndex("LoanEmployeeId");

                    b.HasIndex("LoanPaymentTypeId");

                    b.HasIndex("LoanUserId");

                    b.ToTable("Loans");
                });

            modelBuilder.Entity("practicaPrestamos4.Entidades.LoanHistory", b =>
                {
                    b.Property<int>("LoanHistoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LoanHistoryId"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("FieldChanged")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<byte>("LoanHistoryStatus")
                        .HasColumnType("tinyint");

                    b.Property<int>("LoanHistoryUserId")
                        .HasColumnType("int");

                    b.Property<long>("LoanId")
                        .HasColumnType("bigint");

                    b.Property<long?>("LoanId1")
                        .HasColumnType("bigint");

                    b.Property<string>("NewValue")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("OldValue")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("LoanHistoryId");

                    b.HasIndex("LoanHistoryUserId");

                    b.HasIndex("LoanId");

                    b.HasIndex("LoanId1");

                    b.ToTable("LoanHistory", (string)null);
                });

            modelBuilder.Entity("practicaPrestamos4.Entidades.PaymentType", b =>
                {
                    b.Property<int>("PaymentTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PaymentTypeId"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)")
                        .HasColumnName("Description");

                    b.Property<string>("ShortName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("ShortName");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("PaymentTypeId");

                    b.ToTable("PaymentTypes");

                    b.HasData(
                        new
                        {
                            PaymentTypeId = 1,
                            CreatedAt = new DateTime(2025, 3, 9, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Pago semanal",
                            ShortName = "Semanal",
                            UpdatedAt = new DateTime(2025, 3, 9, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            PaymentTypeId = 2,
                            CreatedAt = new DateTime(2025, 3, 9, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Pago cada catorce días",
                            ShortName = "Catorcenal",
                            UpdatedAt = new DateTime(2025, 3, 9, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            PaymentTypeId = 3,
                            CreatedAt = new DateTime(2025, 3, 9, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Pago cada quince días",
                            ShortName = "Quincenal",
                            UpdatedAt = new DateTime(2025, 3, 9, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            PaymentTypeId = 4,
                            CreatedAt = new DateTime(2025, 3, 9, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Pago mensual",
                            ShortName = "Mensual",
                            UpdatedAt = new DateTime(2025, 3, 9, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            PaymentTypeId = 5,
                            CreatedAt = new DateTime(2025, 3, 9, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Pago único al final del préstamo",
                            ShortName = "Al Final",
                            UpdatedAt = new DateTime(2025, 3, 9, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        });
                });

            modelBuilder.Entity("practicaPrestamos4.Entidades.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedEmail")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<byte>("UserStatus")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint")
                        .HasDefaultValue((byte)1);

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("practicaPrestamos4.Entidades.Loan", b =>
                {
                    b.HasOne("practicaPrestamos4.Entidades.Employee", "Employee")
                        .WithMany("Loans")
                        .HasForeignKey("LoanEmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("practicaPrestamos4.Entidades.PaymentType", "PaymentTypes")
                        .WithMany()
                        .HasForeignKey("LoanPaymentTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("practicaPrestamos4.Entidades.User", "User")
                        .WithMany()
                        .HasForeignKey("LoanUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");

                    b.Navigation("PaymentTypes");

                    b.Navigation("User");
                });

            modelBuilder.Entity("practicaPrestamos4.Entidades.LoanHistory", b =>
                {
                    b.HasOne("practicaPrestamos4.Entidades.User", "User")
                        .WithMany()
                        .HasForeignKey("LoanHistoryUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("practicaPrestamos4.Entidades.Loan", "Loan")
                        .WithMany()
                        .HasForeignKey("LoanId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("practicaPrestamos4.Entidades.Loan", null)
                        .WithMany("LoanHistories")
                        .HasForeignKey("LoanId1");

                    b.Navigation("Loan");

                    b.Navigation("User");
                });

            modelBuilder.Entity("practicaPrestamos4.Entidades.Employee", b =>
                {
                    b.Navigation("Loans");
                });

            modelBuilder.Entity("practicaPrestamos4.Entidades.Loan", b =>
                {
                    b.Navigation("LoanHistories");
                });
#pragma warning restore 612, 618
        }
    }
}
