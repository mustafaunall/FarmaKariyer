using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class db_10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LicenseRightLeft",
                table: "Adverts",
                newName: "WithoutLicenseRight");

            migrationBuilder.AddColumn<bool>(
                name: "IsDermocosmetic",
                table: "Adverts",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "WithLicenseRight",
                table: "Adverts",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AdvertCategories",
                keyColumn: "Id",
                keyValue: 1,
                column: "Price",
                value: 500f);

            migrationBuilder.UpdateData(
                table: "AdvertCategories",
                keyColumn: "Id",
                keyValue: 2,
                column: "Price",
                value: 1000f);

            migrationBuilder.UpdateData(
                table: "AdvertCategories",
                keyColumn: "Id",
                keyValue: 3,
                column: "Price",
                value: 3000f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDermocosmetic",
                table: "Adverts");

            migrationBuilder.DropColumn(
                name: "WithLicenseRight",
                table: "Adverts");

            migrationBuilder.RenameColumn(
                name: "WithoutLicenseRight",
                table: "Adverts",
                newName: "LicenseRightLeft");

            migrationBuilder.UpdateData(
                table: "AdvertCategories",
                keyColumn: "Id",
                keyValue: 1,
                column: "Price",
                value: 150f);

            migrationBuilder.UpdateData(
                table: "AdvertCategories",
                keyColumn: "Id",
                keyValue: 2,
                column: "Price",
                value: 380f);

            migrationBuilder.UpdateData(
                table: "AdvertCategories",
                keyColumn: "Id",
                keyValue: 3,
                column: "Price",
                value: 2500f);
        }
    }
}
