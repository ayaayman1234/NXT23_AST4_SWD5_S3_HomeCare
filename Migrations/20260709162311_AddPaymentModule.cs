using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NursingCarePlatform.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_CareRequests_CareRequestId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_CareRequestId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_NursingNotes_AssignmentId",
                table: "NursingNotes");

            migrationBuilder.DropIndex(
                name: "IX_MedicalChecklists_CareRequestId",
                table: "MedicalChecklists");

            migrationBuilder.AlterColumn<string>(
                name: "TransactionReference",
                table: "Payments",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "PaymentStatus",
                table: "Payments",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "PaymentMethod",
                table: "Payments",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<decimal>(
                name: "CommissionAmount",
                table: "Payments",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "NetAmount",
                table: "Payments",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CareRequestId",
                table: "Payments",
                column: "CareRequestId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NursingNotes_AssignmentId",
                table: "NursingNotes",
                column: "AssignmentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MedicalChecklists_CareRequestId",
                table: "MedicalChecklists",
                column: "CareRequestId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_CareRequests_CareRequestId",
                table: "Payments",
                column: "CareRequestId",
                principalTable: "CareRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_CareRequests_CareRequestId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_CareRequestId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_NursingNotes_AssignmentId",
                table: "NursingNotes");

            migrationBuilder.DropIndex(
                name: "IX_MedicalChecklists_CareRequestId",
                table: "MedicalChecklists");

            migrationBuilder.DropColumn(
                name: "CommissionAmount",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "NetAmount",
                table: "Payments");

            migrationBuilder.AlterColumn<string>(
                name: "TransactionReference",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "PaymentStatus",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "PaymentMethod",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CareRequestId",
                table: "Payments",
                column: "CareRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_NursingNotes_AssignmentId",
                table: "NursingNotes",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalChecklists_CareRequestId",
                table: "MedicalChecklists",
                column: "CareRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_CareRequests_CareRequestId",
                table: "Payments",
                column: "CareRequestId",
                principalTable: "CareRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
