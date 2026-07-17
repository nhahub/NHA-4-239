using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HouraDAL.Migrations
{
    /// <inheritdoc />
    public partial class edit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Bio",
                table: "USERS",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Skills",
                table: "USERS",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "FiatPackages",
                columns: new[] { "PackageId", "HoursCount", "PackageName", "Price" },
                values: new object[,]
                {
                    { 1, 2, "2 hours", 5.00m },
                    { 2, 5, "5 hours", 12.00m },
                    { 3, 10, "10 hours", 20.00m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "FiatPackages",
                keyColumn: "PackageId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "FiatPackages",
                keyColumn: "PackageId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "FiatPackages",
                keyColumn: "PackageId",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "Bio",
                table: "USERS");

            migrationBuilder.DropColumn(
                name: "Skills",
                table: "USERS");
        }
    }
}
