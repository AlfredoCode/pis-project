using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;

#nullable disable

namespace PRegSys.DAL.Migrations
{
    /// <inheritdoc />
    public partial class MultipleSolutions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_solutions_team_id",
                table: "solutions");

            migrationBuilder.UpdateData(
                table: "solutions",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "evaluation_id", "file", "submission_date" },
                values: new object[] { null, "old"u8.ToArray(), NodaTime.Instant.FromUnixTimeTicks(16967484000000000L) });

            migrationBuilder.InsertData(
                table: "solutions",
                columns: new[] { "id", "evaluation_id", "file", "project_id", "submission_date", "team_id" },
                values: new object[] { 2, 1, "new"u8.ToArray(), 1, NodaTime.Instant.FromUnixTimeTicks(16969392000000000L), 1 });

            migrationBuilder.CreateIndex(
                name: "ix_solutions_team_id",
                table: "solutions",
                column: "team_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_solutions_team_id",
                table: "solutions");

            migrationBuilder.DeleteData(
                table: "solutions",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "solutions",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "evaluation_id", "file", "submission_date" },
                values: new object[] { 1, new byte[] { 0 }, NodaTime.Instant.FromUnixTimeTicks(16961616000000000L) });

            migrationBuilder.CreateIndex(
                name: "ix_solutions_team_id",
                table: "solutions",
                column: "team_id",
                unique: true);
        }
    }
}
