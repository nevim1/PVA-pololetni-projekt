using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bigWork.Migrations
{
    /// <inheritdoc />
    public partial class getSetters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Employee",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Surname",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "BorrowedBookBookId",
                table: "Borrows",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateBorrowed",
                table: "Borrows",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateReturned",
                table: "Borrows",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ReturnDate",
                table: "Borrows",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Borrows",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Autor",
                table: "Books",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "Books",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Genre",
                table: "Books",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ISBN",
                table: "Books",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Books",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ReleaseYear",
                table: "Books",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Borrows_BorrowedBookBookId",
                table: "Borrows",
                column: "BorrowedBookBookId");

            migrationBuilder.CreateIndex(
                name: "IX_Borrows_Username",
                table: "Borrows",
                column: "Username");

            migrationBuilder.AddForeignKey(
                name: "FK_Borrows_Books_BorrowedBookBookId",
                table: "Borrows",
                column: "BorrowedBookBookId",
                principalTable: "Books",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Borrows_Users_Username",
                table: "Borrows",
                column: "Username",
                principalTable: "Users",
                principalColumn: "Username");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Borrows_Books_BorrowedBookBookId",
                table: "Borrows");

            migrationBuilder.DropForeignKey(
                name: "FK_Borrows_Users_Username",
                table: "Borrows");

            migrationBuilder.DropIndex(
                name: "IX_Borrows_BorrowedBookBookId",
                table: "Borrows");

            migrationBuilder.DropIndex(
                name: "IX_Borrows_Username",
                table: "Borrows");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Employee",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Surname",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "BorrowedBookBookId",
                table: "Borrows");

            migrationBuilder.DropColumn(
                name: "DateBorrowed",
                table: "Borrows");

            migrationBuilder.DropColumn(
                name: "DateReturned",
                table: "Borrows");

            migrationBuilder.DropColumn(
                name: "ReturnDate",
                table: "Borrows");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Borrows");

            migrationBuilder.DropColumn(
                name: "Autor",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "Count",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "Genre",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "ISBN",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "ReleaseYear",
                table: "Books");
        }
    }
}
