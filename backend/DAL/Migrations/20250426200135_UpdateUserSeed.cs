using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRegSys.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "password",
                table: "users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                column: "password",
                value: "GuExk1o8330FmzAc3LNRRjqutOq+kfcH6QfDFEyfiCWPdcbe");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 2,
                column: "password",
                value: "GuExk1o8330FmzAc3LNRRjqutOq+kfcH6QfDFEyfiCWPdcbe");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 3,
                column: "password",
                value: "GuExk1o8330FmzAc3LNRRjqutOq+kfcH6QfDFEyfiCWPdcbe");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 4,
                column: "password",
                value: "GuExk1o8330FmzAc3LNRRjqutOq+kfcH6QfDFEyfiCWPdcbe");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 5,
                column: "password",
                value: "GuExk1o8330FmzAc3LNRRjqutOq+kfcH6QfDFEyfiCWPdcbe");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 6,
                column: "password",
                value: "GuExk1o8330FmzAc3LNRRjqutOq+kfcH6QfDFEyfiCWPdcbe");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 7,
                column: "password",
                value: "GuExk1o8330FmzAc3LNRRjqutOq+kfcH6QfDFEyfiCWPdcbe");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 8,
                column: "password",
                value: "GuExk1o8330FmzAc3LNRRjqutOq+kfcH6QfDFEyfiCWPdcbe");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "password",
                table: "users");
        }
    }
}
