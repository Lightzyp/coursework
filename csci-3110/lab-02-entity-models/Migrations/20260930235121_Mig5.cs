using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Lab02Models.Migrations
{
    /// <inheritdoc />
    public partial class Mig5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Buildings",
                columns: new[] { "BuildingNumber", "Name" },
                values: new object[,]
                {
                    { 100, "Science Hall" },
                    { 200, "Technology Center" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "name" },
                values: new object[] { 1, "Electronics" });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Code", "Number", "CreditHours" },
                values: new object[,]
                {
                    { "CSCI", "1250", 4 },
                    { "CSCI", "3110", 3 }
                });

            migrationBuilder.InsertData(
                table: "Department",
                columns: new[] { "Id", "Budget", "Name" },
                values: new object[] { 1, 100000m, "Information Technology" });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Website Redesign" },
                    { 2, "Inventory System" }
                });

            migrationBuilder.InsertData(
                table: "Employee",
                columns: new[] { "Id", "DepartmentId", "FirstName", "LastName" },
                values: new object[,]
                {
                    { 1, 1, "Alex", "Smith" },
                    { 2, 1, "Jordan", "Lee" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "Price", "name" },
                values: new object[,]
                {
                    { 1, 1, 49.99m, "Keyboard" },
                    { 2, 1, 24.99m, "Mouse" }
                });

            migrationBuilder.InsertData(
                table: "Assignments",
                columns: new[] { "EmployeeId", "ProjectId", "Hours", "Role" },
                values: new object[,]
                {
                    { 1, 1, 20m, "Developer" },
                    { 1, 2, 10m, "Tester" },
                    { 2, 1, 15m, "Designer" }
                });

            migrationBuilder.InsertData(
                table: "Lockers",
                columns: new[] { "Id", "EmployeeId", "Number" },
                values: new object[] { 1, 1, "L101" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Assignments",
                keyColumns: new[] { "EmployeeId", "ProjectId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "Assignments",
                keyColumns: new[] { "EmployeeId", "ProjectId" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "Assignments",
                keyColumns: new[] { "EmployeeId", "ProjectId" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "Buildings",
                keyColumn: "BuildingNumber",
                keyValue: 100);

            migrationBuilder.DeleteData(
                table: "Buildings",
                keyColumn: "BuildingNumber",
                keyValue: 200);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumns: new[] { "Code", "Number" },
                keyValues: new object[] { "CSCI", "1250" });

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumns: new[] { "Code", "Number" },
                keyValues: new object[] { "CSCI", "3110" });

            migrationBuilder.DeleteData(
                table: "Lockers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Employee",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Employee",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Department",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
