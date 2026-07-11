using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NursingCarePlatform.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddIsApprovedToNurseDocument : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "NurseDocuments",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "NurseDocuments");
        }
    }
}
