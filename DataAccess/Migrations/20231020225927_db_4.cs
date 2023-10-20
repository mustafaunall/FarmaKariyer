using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class db_4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrentResumeId",
                table: "Applies",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Applies_CurrentResumeId",
                table: "Applies",
                column: "CurrentResumeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applies_Resumes_CurrentResumeId",
                table: "Applies",
                column: "CurrentResumeId",
                principalTable: "Resumes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applies_Resumes_CurrentResumeId",
                table: "Applies");

            migrationBuilder.DropIndex(
                name: "IX_Applies_CurrentResumeId",
                table: "Applies");

            migrationBuilder.DropColumn(
                name: "CurrentResumeId",
                table: "Applies");
        }
    }
}
