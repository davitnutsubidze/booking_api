using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Booking_API.Migrations
{
    /// <inheritdoc />
    public partial class SeedCompany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedDate",
                table: "Company",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Company",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.InsertData(
                table: "Company",
                columns: new[] { "Id", "CreatedDate", "Details", "Name", "Rate", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 2, 9, 12, 0, 0, 0, DateTimeKind.Utc), "Software development company", "Alpha Tech", 4.5, new DateTime(2026, 2, 9, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, new DateTime(2026, 2, 9, 12, 5, 0, 0, DateTimeKind.Utc), "IT consulting services", "Beta Solutions", 4.2000000000000002, new DateTime(2026, 2, 9, 12, 5, 0, 0, DateTimeKind.Utc) },
                    { 3, new DateTime(2026, 2, 9, 12, 10, 0, 0, DateTimeKind.Utc), "Business analytics and BI", "Gamma Group", 4.7999999999999998, new DateTime(2026, 2, 9, 12, 10, 0, 0, DateTimeKind.Utc) },
                    { 4, new DateTime(2026, 2, 9, 12, 15, 0, 0, DateTimeKind.Utc), "Cloud infrastructure services", "Delta Systems", 4.0, new DateTime(2026, 2, 9, 12, 15, 0, 0, DateTimeKind.Utc) },
                    { 5, new DateTime(2026, 2, 9, 12, 20, 0, 0, DateTimeKind.Utc), "AI & machine learning startup", "Epsilon Labs", 4.9000000000000004, new DateTime(2026, 2, 9, 12, 20, 0, 0, DateTimeKind.Utc) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Company",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Company",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Company",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Company",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Company",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedDate",
                table: "Company",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Company",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);
        }
    }
}
