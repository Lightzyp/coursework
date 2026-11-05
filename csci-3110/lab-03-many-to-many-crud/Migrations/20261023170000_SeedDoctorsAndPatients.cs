using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DMLab3CRRUDM2M.Migrations
{
    /// <inheritdoc />
    public partial class SeedDoctorsAndPatients : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Doctors",
                columns: new[] { "Id", "Name", "Specialty" },
                values: new object[,]
                {
                    { 101, "Dr. Amara Okafor", "Cardiology" },
                    { 102, "Dr. Benjamin Hayes", "Pediatrics" },
                    { 103, "Dr. Carmen Lopez", "Dermatology" },
                    { 104, "Dr. David Chen", "Family Medicine" },
                    { 105, "Dr. Elena Petrova", "Neurology" }
                });

            migrationBuilder.InsertData(
                table: "Patients",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 201, "Frank Miller" },
                    { 202, "Grace Kim" },
                    { 203, "Hector Ramirez" },
                    { 204, "Isabel Turner" },
                    { 205, "Jamal Wright" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(table: "Doctors", keyColumn: "Id", keyValue: 101);
            migrationBuilder.DeleteData(table: "Doctors", keyColumn: "Id", keyValue: 102);
            migrationBuilder.DeleteData(table: "Doctors", keyColumn: "Id", keyValue: 103);
            migrationBuilder.DeleteData(table: "Doctors", keyColumn: "Id", keyValue: 104);
            migrationBuilder.DeleteData(table: "Doctors", keyColumn: "Id", keyValue: 105);

            migrationBuilder.DeleteData(table: "Patients", keyColumn: "Id", keyValue: 201);
            migrationBuilder.DeleteData(table: "Patients", keyColumn: "Id", keyValue: 202);
            migrationBuilder.DeleteData(table: "Patients", keyColumn: "Id", keyValue: 203);
            migrationBuilder.DeleteData(table: "Patients", keyColumn: "Id", keyValue: 204);
            migrationBuilder.DeleteData(table: "Patients", keyColumn: "Id", keyValue: 205);
        }
    }
}
