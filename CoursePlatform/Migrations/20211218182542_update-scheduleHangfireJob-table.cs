using Microsoft.EntityFrameworkCore.Migrations;

namespace CoursesPlatform.Migrations
{
    public partial class updatescheduleHangfireJobtable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecieverEmail",
                table: "ScheduleHangfireJobs");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "ScheduleHangfireJobs",
                newName: "UserSubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleHangfireJobs_UserSubscriptionId",
                table: "ScheduleHangfireJobs",
                column: "UserSubscriptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduleHangfireJobs_tblUserSubscriptions_UserSubscriptionId",
                table: "ScheduleHangfireJobs",
                column: "UserSubscriptionId",
                principalTable: "tblUserSubscriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScheduleHangfireJobs_tblUserSubscriptions_UserSubscriptionId",
                table: "ScheduleHangfireJobs");

            migrationBuilder.DropIndex(
                name: "IX_ScheduleHangfireJobs_UserSubscriptionId",
                table: "ScheduleHangfireJobs");

            migrationBuilder.RenameColumn(
                name: "UserSubscriptionId",
                table: "ScheduleHangfireJobs",
                newName: "CourseId");

            migrationBuilder.AddColumn<string>(
                name: "RecieverEmail",
                table: "ScheduleHangfireJobs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
