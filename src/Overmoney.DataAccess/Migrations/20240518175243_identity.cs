using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Overmoney.Api.Migrations
{
    /// <inheritdoc />
    public partial class identity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Login",
                schema: "overmoney_api",
                table: "users");

            migrationBuilder.DropColumn(
                name: "login",
                schema: "overmoney_api",
                table: "users");

            migrationBuilder.DropColumn(
                name: "password",
                schema: "overmoney_api",
                table: "users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "login",
                schema: "overmoney_api",
                table: "users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "password",
                schema: "overmoney_api",
                table: "users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Login",
                schema: "overmoney_api",
                table: "users",
                column: "login",
                unique: true);
        }
    }
}
