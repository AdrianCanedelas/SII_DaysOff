using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SII_DaysOff.Migrations
{
    public partial class allowDuplicatedCreatedBy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CreatedBy",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_Manager",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ModifiedBy",
                table: "AspNetUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CreatedBy",
                table: "AspNetUsers",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Manager",
                table: "AspNetUsers",
                column: "Manager");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ModifiedBy",
                table: "AspNetUsers",
                column: "ModifiedBy");
        }
    }
}
