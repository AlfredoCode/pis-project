using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRegSys.DAL.Migrations
{
    /// <inheritdoc />
    public partial class EntityFixups : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_sign_requests_students_student_id",
                table: "sign_requests");

            migrationBuilder.DropForeignKey(
                name: "fk_sign_requests_teams_team_id",
                table: "sign_requests");

            migrationBuilder.DropForeignKey(
                name: "fk_solutions_evaluations_evaluation_id",
                table: "solutions");

            migrationBuilder.DropPrimaryKey(
                name: "pk_sign_requests",
                table: "sign_requests");

            migrationBuilder.DropColumn(
                name: "size",
                table: "teams");

            migrationBuilder.RenameTable(
                name: "sign_requests",
                newName: "sign_up_requests");

            migrationBuilder.RenameIndex(
                name: "ix_sign_requests_team_id",
                table: "sign_up_requests",
                newName: "ix_sign_up_requests_team_id");

            migrationBuilder.RenameIndex(
                name: "ix_sign_requests_student_id",
                table: "sign_up_requests",
                newName: "ix_sign_up_requests_student_id");

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "teams",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "evaluation_id",
                table: "solutions",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddPrimaryKey(
                name: "pk_sign_up_requests",
                table: "sign_up_requests",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_sign_up_requests_students_student_id",
                table: "sign_up_requests",
                column: "student_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_sign_up_requests_teams_team_id",
                table: "sign_up_requests",
                column: "team_id",
                principalTable: "teams",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_solutions_evaluations_evaluation_id",
                table: "solutions",
                column: "evaluation_id",
                principalTable: "evaluations",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_sign_up_requests_students_student_id",
                table: "sign_up_requests");

            migrationBuilder.DropForeignKey(
                name: "fk_sign_up_requests_teams_team_id",
                table: "sign_up_requests");

            migrationBuilder.DropForeignKey(
                name: "fk_solutions_evaluations_evaluation_id",
                table: "solutions");

            migrationBuilder.DropPrimaryKey(
                name: "pk_sign_up_requests",
                table: "sign_up_requests");

            migrationBuilder.DropColumn(
                name: "name",
                table: "teams");

            migrationBuilder.RenameTable(
                name: "sign_up_requests",
                newName: "sign_requests");

            migrationBuilder.RenameIndex(
                name: "ix_sign_up_requests_team_id",
                table: "sign_requests",
                newName: "ix_sign_requests_team_id");

            migrationBuilder.RenameIndex(
                name: "ix_sign_up_requests_student_id",
                table: "sign_requests",
                newName: "ix_sign_requests_student_id");

            migrationBuilder.AddColumn<int>(
                name: "size",
                table: "teams",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "evaluation_id",
                table: "solutions",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "pk_sign_requests",
                table: "sign_requests",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_sign_requests_students_student_id",
                table: "sign_requests",
                column: "student_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_sign_requests_teams_team_id",
                table: "sign_requests",
                column: "team_id",
                principalTable: "teams",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_solutions_evaluations_evaluation_id",
                table: "solutions",
                column: "evaluation_id",
                principalTable: "evaluations",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
