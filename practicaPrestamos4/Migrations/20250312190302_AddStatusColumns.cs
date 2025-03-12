using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace practicaPrestamos4.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "LoanStatus",
                table: "Loans",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)1);

            migrationBuilder.AddColumn<byte>(
                name: "LoanHistoryStatus",
                table: "LoanHistory",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoanStatus",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "LoanHistoryStatus",
                table: "LoanHistory");
        }
    }
}
