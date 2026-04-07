using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bigWork.Migrations
{
    /// <inheritdoc />
    public partial class bookUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Count",
                table: "Books");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "Books",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
