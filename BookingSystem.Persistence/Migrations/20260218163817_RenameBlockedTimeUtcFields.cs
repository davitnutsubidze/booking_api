using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingSystem.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RenameBlockedTimeUtcFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlockedTime_Staff_StaffId",
                table: "BlockedTime");

            migrationBuilder.DropForeignKey(
                name: "FK_BlockedTime_Tenant_BusinessId",
                table: "BlockedTime");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BlockedTime",
                table: "BlockedTime");

            migrationBuilder.RenameTable(
                name: "BlockedTime",
                newName: "BlockedTimes");

            migrationBuilder.RenameColumn(
                name: "StartDateTime",
                table: "BlockedTimes",
                newName: "StartDateTimeUtc");

            migrationBuilder.RenameColumn(
                name: "EndDateTime",
                table: "BlockedTimes",
                newName: "EndDateTimeUtc");

            migrationBuilder.RenameIndex(
                name: "IX_BlockedTime_StaffId",
                table: "BlockedTimes",
                newName: "IX_BlockedTimes_StaffId");

            migrationBuilder.RenameIndex(
                name: "IX_BlockedTime_BusinessId",
                table: "BlockedTimes",
                newName: "IX_BlockedTimes_BusinessId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlockedTimes",
                table: "BlockedTimes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BlockedTimes_Staff_StaffId",
                table: "BlockedTimes",
                column: "StaffId",
                principalTable: "Staff",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BlockedTimes_Tenant_BusinessId",
                table: "BlockedTimes",
                column: "TenantId",
                principalTable: "Tenant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlockedTimes_Staff_StaffId",
                table: "BlockedTimes");

            migrationBuilder.DropForeignKey(
                name: "FK_BlockedTimes_Tenant_BusinessId",
                table: "BlockedTimes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BlockedTimes",
                table: "BlockedTimes");

            migrationBuilder.RenameTable(
                name: "BlockedTimes",
                newName: "BlockedTime");

            migrationBuilder.RenameColumn(
                name: "StartDateTimeUtc",
                table: "BlockedTime",
                newName: "StartDateTime");

            migrationBuilder.RenameColumn(
                name: "EndDateTimeUtc",
                table: "BlockedTime",
                newName: "EndDateTime");

            migrationBuilder.RenameIndex(
                name: "IX_BlockedTimes_StaffId",
                table: "BlockedTime",
                newName: "IX_BlockedTime_StaffId");

            migrationBuilder.RenameIndex(
                name: "IX_BlockedTimes_BusinessId",
                table: "BlockedTime",
                newName: "IX_BlockedTime_BusinessId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlockedTime",
                table: "BlockedTime",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BlockedTime_Staff_StaffId",
                table: "BlockedTime",
                column: "StaffId",
                principalTable: "Staff",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BlockedTime_Tenant_BusinessId",
                table: "BlockedTime",
                column: "TenantId",
                principalTable: "Tenant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
