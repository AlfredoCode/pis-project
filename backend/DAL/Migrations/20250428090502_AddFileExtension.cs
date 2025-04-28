using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRegSys.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddFileExtension : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "file_extension",
                table: "solutions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "solutions",
                keyColumn: "id",
                keyValue: 1,
                column: "file_extension",
                value: "txt");

            migrationBuilder.UpdateData(
                table: "solutions",
                keyColumn: "id",
                keyValue: 2,
                column: "file_extension",
                value: "txt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "file_extension",
                table: "solutions");
        }
    }
}
