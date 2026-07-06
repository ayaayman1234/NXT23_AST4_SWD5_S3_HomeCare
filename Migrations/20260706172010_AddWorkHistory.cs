using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NursingCarePlatform.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkHistories_Assignments_AssignmentId",
                table: "WorkHistories");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "WorkHistories");

            migrationBuilder.DropColumn(
                name: "JobStatus",
                table: "WorkHistories");

            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "WorkHistories",
                newName: "CompletedAt");

            migrationBuilder.RenameColumn(
                name: "AssignmentId",
                table: "WorkHistories",
                newName: "ServiceId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkHistories_AssignmentId",
                table: "WorkHistories",
                newName: "IX_WorkHistories_ServiceId");

            migrationBuilder.AddColumn<int>(
                name: "CareRequestId",
                table: "WorkHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NurseId",
                table: "WorkHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PatientId",
                table: "WorkHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RequiredHours",
                table: "WorkHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "WorkHistories",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedAt",
                table: "Assignments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkHistories_CareRequestId",
                table: "WorkHistories",
                column: "CareRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkHistories_NurseId",
                table: "WorkHistories",
                column: "NurseId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkHistories_PatientId",
                table: "WorkHistories",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkHistories_CareRequests_CareRequestId",
                table: "WorkHistories",
                column: "CareRequestId",
                principalTable: "CareRequests",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkHistories_Nurses_NurseId",
                table: "WorkHistories",
                column: "NurseId",
                principalTable: "Nurses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkHistories_NursingServices_ServiceId",
                table: "WorkHistories",
                column: "ServiceId",
                principalTable: "NursingServices",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkHistories_Patients_PatientId",
                table: "WorkHistories",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkHistories_CareRequests_CareRequestId",
                table: "WorkHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkHistories_Nurses_NurseId",
                table: "WorkHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkHistories_NursingServices_ServiceId",
                table: "WorkHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkHistories_Patients_PatientId",
                table: "WorkHistories");

            migrationBuilder.DropIndex(
                name: "IX_WorkHistories_CareRequestId",
                table: "WorkHistories");

            migrationBuilder.DropIndex(
                name: "IX_WorkHistories_NurseId",
                table: "WorkHistories");

            migrationBuilder.DropIndex(
                name: "IX_WorkHistories_PatientId",
                table: "WorkHistories");

            migrationBuilder.DropColumn(
                name: "CareRequestId",
                table: "WorkHistories");

            migrationBuilder.DropColumn(
                name: "NurseId",
                table: "WorkHistories");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "WorkHistories");

            migrationBuilder.DropColumn(
                name: "RequiredHours",
                table: "WorkHistories");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "WorkHistories");

            migrationBuilder.DropColumn(
                name: "CompletedAt",
                table: "Assignments");

            migrationBuilder.RenameColumn(
                name: "ServiceId",
                table: "WorkHistories",
                newName: "AssignmentId");

            migrationBuilder.RenameColumn(
                name: "CompletedAt",
                table: "WorkHistories",
                newName: "StartTime");

            migrationBuilder.RenameIndex(
                name: "IX_WorkHistories_ServiceId",
                table: "WorkHistories",
                newName: "IX_WorkHistories_AssignmentId");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                table: "WorkHistories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "JobStatus",
                table: "WorkHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkHistories_Assignments_AssignmentId",
                table: "WorkHistories",
                column: "AssignmentId",
                principalTable: "Assignments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
