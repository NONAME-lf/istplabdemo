using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixCartUniqueIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_carts_user_id_unique",
                table: "carts");

            migrationBuilder.DropIndex(
                name: "IX_carts_session_id_unique",
                table: "carts");

            migrationBuilder.CreateIndex(
                name: "IX_carts_user_id_unique",
                table: "carts",
                column: "user_id",
                unique: true,
                filter: "[user_id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_carts_session_id_unique",
                table: "carts",
                column: "session_id",
                unique: true,
                filter: "[session_id] IS NOT NULL");


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
