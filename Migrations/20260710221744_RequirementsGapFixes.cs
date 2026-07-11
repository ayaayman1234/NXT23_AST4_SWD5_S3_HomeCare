using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NursingCarePlatform.Web.Migrations
{
    /// <inheritdoc />
    public partial class RequirementsGapFixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssignmentId",
                table: "WorkHistories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JobStatus",
                table: "WorkHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "StartTime",
                table: "WorkHistories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "RecurringRequests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RatedUserGuid",
                table: "Ratings",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RaterUserGuid",
                table: "Ratings",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AgainstUserId",
                table: "Complaints",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ComplaintCareRequestId",
                table: "Complaints",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "Complaints",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SubscriptionPlans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    MonthlyFee = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CommissionRate = table.Column<decimal>(type: "decimal(5,4)", precision: 5, scale: 4, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionPlans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NurseSubscriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NurseId = table.Column<int>(type: "int", nullable: false),
                    PlanId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NurseSubscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NurseSubscriptions_Nurses_NurseId",
                        column: x => x.NurseId,
                        principalTable: "Nurses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NurseSubscriptions_SubscriptionPlans_PlanId",
                        column: x => x.PlanId,
                        principalTable: "SubscriptionPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkHistories_AssignmentId",
                table: "WorkHistories",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_RatedUserGuid",
                table: "Ratings",
                column: "RatedUserGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_RaterUserGuid",
                table: "Ratings",
                column: "RaterUserGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Complaints_AgainstUserId",
                table: "Complaints",
                column: "AgainstUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Complaints_ComplaintCareRequestId",
                table: "Complaints",
                column: "ComplaintCareRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Complaints_CreatedByUserId",
                table: "Complaints",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NurseSubscriptions_NurseId",
                table: "NurseSubscriptions",
                column: "NurseId");

            migrationBuilder.CreateIndex(
                name: "IX_NurseSubscriptions_PlanId",
                table: "NurseSubscriptions",
                column: "PlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_Complaints_AspNetUsers_AgainstUserId",
                table: "Complaints",
                column: "AgainstUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Complaints_AspNetUsers_CreatedByUserId",
                table: "Complaints",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Complaints_CareRequests_ComplaintCareRequestId",
                table: "Complaints",
                column: "ComplaintCareRequestId",
                principalTable: "CareRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_AspNetUsers_RatedUserGuid",
                table: "Ratings",
                column: "RatedUserGuid",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_AspNetUsers_RaterUserGuid",
                table: "Ratings",
                column: "RaterUserGuid",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkHistories_Assignments_AssignmentId",
                table: "WorkHistories",
                column: "AssignmentId",
                principalTable: "Assignments",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Complaints_AspNetUsers_AgainstUserId",
                table: "Complaints");

            migrationBuilder.DropForeignKey(
                name: "FK_Complaints_AspNetUsers_CreatedByUserId",
                table: "Complaints");

            migrationBuilder.DropForeignKey(
                name: "FK_Complaints_CareRequests_ComplaintCareRequestId",
                table: "Complaints");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_AspNetUsers_RatedUserGuid",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_AspNetUsers_RaterUserGuid",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkHistories_Assignments_AssignmentId",
                table: "WorkHistories");

            migrationBuilder.DropTable(
                name: "NurseSubscriptions");

            migrationBuilder.DropTable(
                name: "SubscriptionPlans");

            migrationBuilder.DropIndex(
                name: "IX_WorkHistories_AssignmentId",
                table: "WorkHistories");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_RatedUserGuid",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_RaterUserGuid",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Complaints_AgainstUserId",
                table: "Complaints");

            migrationBuilder.DropIndex(
                name: "IX_Complaints_ComplaintCareRequestId",
                table: "Complaints");

            migrationBuilder.DropIndex(
                name: "IX_Complaints_CreatedByUserId",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "AssignmentId",
                table: "WorkHistories");

            migrationBuilder.DropColumn(
                name: "JobStatus",
                table: "WorkHistories");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "WorkHistories");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "RecurringRequests");

            migrationBuilder.DropColumn(
                name: "RatedUserGuid",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "RaterUserGuid",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "AgainstUserId",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "ComplaintCareRequestId",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Complaints");
        }
    }
}
