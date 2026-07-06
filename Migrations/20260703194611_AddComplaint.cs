using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NursingCarePlatform.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddComplaint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CareRequests_NursingServices_ServiceId",
                table: "CareRequests");

            migrationBuilder.DropColumn(
                name: "AgainstUserId",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "ComplaintStatus",
                table: "Complaints");

            migrationBuilder.RenameColumn(
                name: "Reason",
                table: "Complaints",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "CreatedByUserId",
                table: "Complaints",
                newName: "PatientId");

            migrationBuilder.RenameColumn(
                name: "CareRequestId",
                table: "Complaints",
                newName: "NurseId");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Complaints",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Complaints",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Complaints_NurseId",
                table: "Complaints",
                column: "NurseId");

            migrationBuilder.CreateIndex(
                name: "IX_Complaints_PatientId",
                table: "Complaints",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_CareRequests_NursingServices_ServiceId",
                table: "CareRequests",
                column: "ServiceId",
                principalTable: "NursingServices",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Complaints_Nurses_NurseId",
                table: "Complaints",
                column: "NurseId",
                principalTable: "Nurses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Complaints_Patients_PatientId",
                table: "Complaints",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CareRequests_NursingServices_ServiceId",
                table: "CareRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Complaints_Nurses_NurseId",
                table: "Complaints");

            migrationBuilder.DropForeignKey(
                name: "FK_Complaints_Patients_PatientId",
                table: "Complaints");

            migrationBuilder.DropIndex(
                name: "IX_Complaints_NurseId",
                table: "Complaints");

            migrationBuilder.DropIndex(
                name: "IX_Complaints_PatientId",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Complaints");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Complaints",
                newName: "Reason");

            migrationBuilder.RenameColumn(
                name: "PatientId",
                table: "Complaints",
                newName: "CreatedByUserId");

            migrationBuilder.RenameColumn(
                name: "NurseId",
                table: "Complaints",
                newName: "CareRequestId");

            migrationBuilder.AddColumn<int>(
                name: "AgainstUserId",
                table: "Complaints",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ComplaintStatus",
                table: "Complaints",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_CareRequests_NursingServices_ServiceId",
                table: "CareRequests",
                column: "ServiceId",
                principalTable: "NursingServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
