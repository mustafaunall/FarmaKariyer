using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class db_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applies_AspNetUsers_RespondentUserId",
                table: "Applies");

            migrationBuilder.RenameColumn(
                name: "RespondentUserId",
                table: "Applies",
                newName: "AdvertId");

            migrationBuilder.RenameIndex(
                name: "IX_Applies_RespondentUserId",
                table: "Applies",
                newName: "IX_Applies_AdvertId");

            migrationBuilder.AddColumn<DateTime>(
                name: "ApplyDate",
                table: "Applies",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_Applies_Adverts_AdvertId",
                table: "Applies",
                column: "AdvertId",
                principalTable: "Adverts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applies_Adverts_AdvertId",
                table: "Applies");

            migrationBuilder.DropColumn(
                name: "ApplyDate",
                table: "Applies");

            migrationBuilder.RenameColumn(
                name: "AdvertId",
                table: "Applies",
                newName: "RespondentUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Applies_AdvertId",
                table: "Applies",
                newName: "IX_Applies_RespondentUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applies_AspNetUsers_RespondentUserId",
                table: "Applies",
                column: "RespondentUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
