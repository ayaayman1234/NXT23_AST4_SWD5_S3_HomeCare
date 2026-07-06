using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NursingCarePlatform.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddNurseToCareRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRated",
                table: "CareRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "NurseId",
                table: "CareRequests",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CareRequests_NurseId",
                table: "CareRequests",
                column: "NurseId");

            migrationBuilder.AddForeignKey(
                name: "FK_CareRequests_Nurses_NurseId",
                table: "CareRequests",
                column: "NurseId",
                principalTable: "Nurses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CareRequests_Nurses_NurseId",
                table: "CareRequests");

            migrationBuilder.DropIndex(
                name: "IX_CareRequests_NurseId",
                table: "CareRequests");

            migrationBuilder.DropColumn(
                name: "IsRated",
                table: "CareRequests");

            migrationBuilder.DropColumn(
                name: "NurseId",
                table: "CareRequests");
        }
    }
}
