using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Overmoney.Api.Migrations
{
    /// <inheritdoc />
    public partial class recurring_transactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_wallets_currencies_user_id",
                schema: "overmoney_api",
                table: "wallets");

            migrationBuilder.CreateTable(
                name: "recurring_transactions",
                schema: "overmoney_api",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    wallet_id = table.Column<int>(type: "integer", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    payee_id = table.Column<int>(type: "integer", nullable: false),
                    category_id = table.Column<int>(type: "integer", nullable: false),
                    transaction_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    transaction_type = table.Column<int>(type: "integer", nullable: false),
                    note = table.Column<string>(type: "text", nullable: true),
                    amount = table.Column<double>(type: "double precision", nullable: false),
                    schedule = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_recurring_transactions", x => x.id);
                    table.ForeignKey(
                        name: "fk_recurring_transactions_categories_category_id",
                        column: x => x.category_id,
                        principalSchema: "overmoney_api",
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_recurring_transactions_payees_payee_id",
                        column: x => x.payee_id,
                        principalSchema: "overmoney_api",
                        principalTable: "payees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_recurring_transactions_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "overmoney_api",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_recurring_transactions_wallets_wallet_id",
                        column: x => x.wallet_id,
                        principalSchema: "overmoney_api",
                        principalTable: "wallets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_wallets_currency_id",
                schema: "overmoney_api",
                table: "wallets",
                column: "currency_id");

            migrationBuilder.CreateIndex(
                name: "ix_recurring_transactions_category_id",
                schema: "overmoney_api",
                table: "recurring_transactions",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "ix_recurring_transactions_payee_id",
                schema: "overmoney_api",
                table: "recurring_transactions",
                column: "payee_id");

            migrationBuilder.CreateIndex(
                name: "ix_recurring_transactions_user_id",
                schema: "overmoney_api",
                table: "recurring_transactions",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_recurring_transactions_wallet_id",
                schema: "overmoney_api",
                table: "recurring_transactions",
                column: "wallet_id");

            migrationBuilder.AddForeignKey(
                name: "fk_wallets_currencies_currency_id",
                schema: "overmoney_api",
                table: "wallets",
                column: "currency_id",
                principalSchema: "overmoney_api",
                principalTable: "currencies",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_wallets_currencies_currency_id",
                schema: "overmoney_api",
                table: "wallets");

            migrationBuilder.DropTable(
                name: "recurring_transactions",
                schema: "overmoney_api");

            migrationBuilder.DropIndex(
                name: "ix_wallets_currency_id",
                schema: "overmoney_api",
                table: "wallets");

            migrationBuilder.AddForeignKey(
                name: "fk_wallets_currencies_user_id",
                schema: "overmoney_api",
                table: "wallets",
                column: "user_id",
                principalSchema: "overmoney_api",
                principalTable: "currencies",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
