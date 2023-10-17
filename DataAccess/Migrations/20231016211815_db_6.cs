using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class db_6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApplicationUserId",
                table: "Adverts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Adverts_ApplicationUserId",
                table: "Adverts",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Adverts_AspNetUsers_ApplicationUserId",
                table: "Adverts",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Adverts_AspNetUsers_ApplicationUserId",
                table: "Adverts");

            migrationBuilder.DropIndex(
                name: "IX_Adverts_ApplicationUserId",
                table: "Adverts");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Adverts");
        }
    }
}
