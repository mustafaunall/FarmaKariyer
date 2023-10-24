using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class db_9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AdvertCategories_AdvertCategoryId",
                table: "Orders");

            migrationBuilder.AlterColumn<int>(
                name: "AdvertCategoryId",
                table: "Orders",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "AdvertId",
                table: "Orders",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderType",
                table: "Orders",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_AdvertId",
                table: "Orders",
                column: "AdvertId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AdvertCategories_AdvertCategoryId",
                table: "Orders",
                column: "AdvertCategoryId",
                principalTable: "AdvertCategories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Adverts_AdvertId",
                table: "Orders",
                column: "AdvertId",
                principalTable: "Adverts",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AdvertCategories_AdvertCategoryId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Adverts_AdvertId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_AdvertId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "AdvertId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderType",
                table: "Orders");

            migrationBuilder.AlterColumn<int>(
                name: "AdvertCategoryId",
                table: "Orders",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AdvertCategories_AdvertCategoryId",
                table: "Orders",
                column: "AdvertCategoryId",
                principalTable: "AdvertCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
