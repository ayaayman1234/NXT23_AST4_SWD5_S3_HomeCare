using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NursingCarePlatform.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddServiceCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "NursingServices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ServiceCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceCategories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_CareRequestId",
                table: "Ratings",
                column: "CareRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_NursingServices_CategoryId",
                table: "NursingServices",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_NursingServices_ServiceCategories_CategoryId",
                table: "NursingServices",
                column: "CategoryId",
                principalTable: "ServiceCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_CareRequests_CareRequestId",
                table: "Ratings",
                column: "CareRequestId",
                principalTable: "CareRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NursingServices_ServiceCategories_CategoryId",
                table: "NursingServices");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_CareRequests_CareRequestId",
                table: "Ratings");

            migrationBuilder.DropTable(
                name: "ServiceCategories");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_CareRequestId",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_NursingServices_CategoryId",
                table: "NursingServices");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "NursingServices");
        }
    }
}
