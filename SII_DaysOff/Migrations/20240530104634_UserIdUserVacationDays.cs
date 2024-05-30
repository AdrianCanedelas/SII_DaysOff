using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SII_DaysOff.Migrations
{
    public partial class UserIdUserVacationDays : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_UserVacationDays_userId",
                table: "UserVacationDays",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserVacationDays_userId",
                table: "UserVacationDays");
        }
    }
}
