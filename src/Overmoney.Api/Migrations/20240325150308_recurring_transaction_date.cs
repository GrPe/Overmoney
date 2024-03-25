using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Overmoney.Api.Migrations
{
    /// <inheritdoc />
    public partial class recurring_transaction_date : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "transaction_date",
                schema: "overmoney_api",
                table: "recurring_transactions",
                newName: "next_occurrence");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "next_occurrence",
                schema: "overmoney_api",
                table: "recurring_transactions",
                newName: "transaction_date");
        }
    }
}
