using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRegSys.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddTeamSolution : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_solutions_team_id",
                table: "solutions");

            migrationBuilder.CreateIndex(
                name: "ix_solutions_team_id",
                table: "solutions",
                column: "team_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_solutions_team_id",
                table: "solutions");

            migrationBuilder.CreateIndex(
                name: "ix_solutions_team_id",
                table: "solutions",
                column: "team_id");
        }
    }
}
