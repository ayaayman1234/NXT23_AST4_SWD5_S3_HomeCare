using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NursingCarePlatform.Web.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCareRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Offers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "CareRequests",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "CareRequests",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "PreferredDate",
                table: "CareRequests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "ServiceId",
                table: "CareRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CareRequests_ServiceId",
                table: "CareRequests",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_CareRequests_NursingServices_ServiceId",
                table: "CareRequests",
                column: "ServiceId",
                principalTable: "NursingServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CareRequests_NursingServices_ServiceId",
                table: "CareRequests");

            migrationBuilder.DropIndex(
                name: "IX_CareRequests_ServiceId",
                table: "CareRequests");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "CareRequests");

            migrationBuilder.DropColumn(
                name: "PreferredDate",
                table: "CareRequests");

            migrationBuilder.DropColumn(
                name: "ServiceId",
                table: "CareRequests");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "CareRequests",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);
        }
    }
}
