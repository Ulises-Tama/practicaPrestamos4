using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace practicaPrestamos4.Migrations
{
    /// <inheritdoc />
    public partial class FixLoanHistoryTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "LoanId1",
                table: "LoanHistory",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LoanHistory_LoanId1",
                table: "LoanHistory",
                column: "LoanId1");

            migrationBuilder.AddForeignKey(
                name: "FK_LoanHistory_Loans_LoanId1",
                table: "LoanHistory",
                column: "LoanId1",
                principalTable: "Loans",
                principalColumn: "LoanId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoanHistory_Loans_LoanId1",
                table: "LoanHistory");

            migrationBuilder.DropIndex(
                name: "IX_LoanHistory_LoanId1",
                table: "LoanHistory");

            migrationBuilder.DropColumn(
                name: "LoanId1",
                table: "LoanHistory");
        }
    }
}
