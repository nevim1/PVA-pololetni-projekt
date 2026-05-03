using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bigWork.Migrations
{
    /// <inheritdoc />
    public partial class ReturningExtending : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CanExtend",
                table: "Borrows",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateReserved",
                table: "Borrows",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanExtend",
                table: "Borrows");

            migrationBuilder.DropColumn(
                name: "DateReserved",
                table: "Borrows");
        }
    }
}
