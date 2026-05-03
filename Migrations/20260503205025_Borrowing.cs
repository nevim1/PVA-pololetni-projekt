using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bigWork.Migrations
{
    /// <inheritdoc />
    public partial class Borrowing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Borrows_Users_Username",
                table: "Borrows");

            migrationBuilder.DropIndex(
                name: "IX_Borrows_Username",
                table: "Borrows");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Borrows");

            migrationBuilder.AddColumn<string>(
                name: "BorrowerUsername",
                table: "Borrows",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Borrows",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Borrows_BorrowerUsername",
                table: "Borrows",
                column: "BorrowerUsername");

            migrationBuilder.AddForeignKey(
                name: "FK_Borrows_Users_BorrowerUsername",
                table: "Borrows",
                column: "BorrowerUsername",
                principalTable: "Users",
                principalColumn: "Username",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Borrows_Users_BorrowerUsername",
                table: "Borrows");

            migrationBuilder.DropIndex(
                name: "IX_Borrows_BorrowerUsername",
                table: "Borrows");

            migrationBuilder.DropColumn(
                name: "BorrowerUsername",
                table: "Borrows");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Borrows");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Borrows",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Borrows_Username",
                table: "Borrows",
                column: "Username");

            migrationBuilder.AddForeignKey(
                name: "FK_Borrows_Users_Username",
                table: "Borrows",
                column: "Username",
                principalTable: "Users",
                principalColumn: "Username");
        }
    }
}
