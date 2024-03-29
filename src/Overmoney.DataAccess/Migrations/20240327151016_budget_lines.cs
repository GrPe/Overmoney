using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Overmoney.Api.Migrations
{
    /// <inheritdoc />
    public partial class budget_lines : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "id",
                schema: "overmoney_api",
                table: "payees",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.CreateTable(
                name: "budgets",
                schema: "overmoney_api",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    year = table.Column<int>(type: "integer", nullable: false),
                    month = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_budgets", x => x.id);
                    table.ForeignKey(
                        name: "fk_budgets_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "overmoney_api",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "budget_lines",
                schema: "overmoney_api",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    budget_id = table.Column<int>(type: "integer", nullable: false),
                    category_id = table.Column<int>(type: "integer", nullable: false),
                    amount = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_budget_lines", x => x.id);
                    table.ForeignKey(
                        name: "fk_budget_lines_budgets_budget_id",
                        column: x => x.budget_id,
                        principalSchema: "overmoney_api",
                        principalTable: "budgets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_budget_lines_categories_category_id",
                        column: x => x.category_id,
                        principalSchema: "overmoney_api",
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_budget_lines_budget_id",
                schema: "overmoney_api",
                table: "budget_lines",
                column: "budget_id");

            migrationBuilder.CreateIndex(
                name: "ix_budget_lines_category_id",
                schema: "overmoney_api",
                table: "budget_lines",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "ix_budgets_user_id",
                schema: "overmoney_api",
                table: "budgets",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "budget_lines",
                schema: "overmoney_api");

            migrationBuilder.DropTable(
                name: "budgets",
                schema: "overmoney_api");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                schema: "overmoney_api",
                table: "payees",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);
        }
    }
}
