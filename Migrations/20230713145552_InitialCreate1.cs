using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nazobook.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Gonderi_UserId",
                table: "Gonderi",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Gonderi_User_UserId",
                table: "Gonderi",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gonderi_User_UserId",
                table: "Gonderi");

            migrationBuilder.DropIndex(
                name: "IX_Gonderi_UserId",
                table: "Gonderi");
        }
    }
}
