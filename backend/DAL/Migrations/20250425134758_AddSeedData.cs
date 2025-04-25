using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PRegSys.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "first_name", "last_name", "username", "user_type" },
                values: new object[,]
                {
                    { 1, "John", "Doe", "jdoe", "Teacher" },
                    { 2, "Alice", "Wonder", "alice", "Student" },
                    { 3, "Bob", "Builder", "bob", "Student" },
                    { 4, "Martin", "Novak", "mnovak", "Teacher" },
                    { 5, "Jana", "Svobodova", "jsvobodova", "Student" },
                    { 6, "Pavel", "Kucera", "pkucera", "Student" },
                    { 7, "Eva", "Horakova", "ehorakova", "Student" },
                    { 8, "Tomas", "Marek", "tmarek", "Student" }
                });

            migrationBuilder.InsertData(
                table: "evaluations",
                columns: new[] { "id", "comment", "points", "teacher_id" },
                values: new object[] { 1, "Good work overall.", 85, 1 });

            migrationBuilder.InsertData(
                table: "projects",
                columns: new[] { "id", "capacity", "course", "deadline", "description", "max_team_size", "name", "owner_id" },
                values: new object[] { 1, 5, "IZP", NodaTime.Instant.FromUnixTimeTicks(17040671990000000L), "Prime number applications using bit fields.", 3, "IZP Projekt 1", 1 });

            migrationBuilder.InsertData(
                table: "teams",
                columns: new[] { "id", "description", "leader_id", "name", "project_id" },
                values: new object[,]
                {
                    { 1, "We do awesome stuff.", 2, "Team Alpha", 1 },
                    { 2, "", 5, "Team Beta", 1 }
                });

            migrationBuilder.InsertData(
                table: "TeamMembers",
                columns: new[] { "students_id", "team_id" },
                values: new object[,]
                {
                    { 2, 1 },
                    { 3, 1 },
                    { 5, 2 },
                    { 6, 2 },
                    { 7, 2 }
                });

            migrationBuilder.InsertData(
                table: "sign_up_requests",
                columns: new[] { "id", "creation_date", "state", "student_id", "team_id" },
                values: new object[] { 1, NodaTime.Instant.FromUnixTimeTicks(16935552000000000L), 0, 3, 1 });

            migrationBuilder.InsertData(
                table: "solutions",
                columns: new[] { "id", "evaluation_id", "file", "project_id", "submission_date", "team_id" },
                values: new object[] { 1, 1, new byte[] { 0 }, 1, NodaTime.Instant.FromUnixTimeTicks(16961616000000000L), 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TeamMembers",
                keyColumns: new[] { "students_id", "team_id" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "TeamMembers",
                keyColumns: new[] { "students_id", "team_id" },
                keyValues: new object[] { 3, 1 });

            migrationBuilder.DeleteData(
                table: "TeamMembers",
                keyColumns: new[] { "students_id", "team_id" },
                keyValues: new object[] { 4, 2 });

            migrationBuilder.DeleteData(
                table: "TeamMembers",
                keyColumns: new[] { "students_id", "team_id" },
                keyValues: new object[] { 5, 2 });

            migrationBuilder.DeleteData(
                table: "TeamMembers",
                keyColumns: new[] { "students_id", "team_id" },
                keyValues: new object[] { 6, 2 });

            migrationBuilder.DeleteData(
                table: "sign_up_requests",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "solutions",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "evaluations",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "teams",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "teams",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "projects",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: 1);
        }
    }
}
