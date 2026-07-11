using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NursingCarePlatform.Web.Migrations
{
    /// <inheritdoc />
    public partial class AdminPreparation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CancelledAt",
                table: "Cancellations",
                newName: "RequestedAt");

            migrationBuilder.AddColumn<string>(
                name: "AdminNotes",
                table: "SOSEvents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsBlocked",
                table: "Patients",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsBlocked",
                table: "Nurses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "AdminNotes",
                table: "Complaints",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminNotes",
                table: "Cancellations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RequestedById",
                table: "Cancellations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RequestedByType",
                table: "Cancellations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Cancellations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_SOSEvents_CareRequestId",
                table: "SOSEvents",
                column: "CareRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_SOSEvents_TriggeredByUserId",
                table: "SOSEvents",
                column: "TriggeredByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SOSEvents_CareRequests_CareRequestId",
                table: "SOSEvents",
                column: "CareRequestId",
                principalTable: "CareRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SOSEvents_Patients_TriggeredByUserId",
                table: "SOSEvents",
                column: "TriggeredByUserId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SOSEvents_CareRequests_CareRequestId",
                table: "SOSEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_SOSEvents_Patients_TriggeredByUserId",
                table: "SOSEvents");

            migrationBuilder.DropIndex(
                name: "IX_SOSEvents_CareRequestId",
                table: "SOSEvents");

            migrationBuilder.DropIndex(
                name: "IX_SOSEvents_TriggeredByUserId",
                table: "SOSEvents");

            migrationBuilder.DropColumn(
                name: "AdminNotes",
                table: "SOSEvents");

            migrationBuilder.DropColumn(
                name: "IsBlocked",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "IsBlocked",
                table: "Nurses");

            migrationBuilder.DropColumn(
                name: "AdminNotes",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "AdminNotes",
                table: "Cancellations");

            migrationBuilder.DropColumn(
                name: "RequestedById",
                table: "Cancellations");

            migrationBuilder.DropColumn(
                name: "RequestedByType",
                table: "Cancellations");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Cancellations");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "RequestedAt",
                table: "Cancellations",
                newName: "CancelledAt");
        }
    }
}
