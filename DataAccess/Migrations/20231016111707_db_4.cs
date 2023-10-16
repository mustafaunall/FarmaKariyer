using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class db_4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Adverts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    ExperienceYear = table.Column<string>(type: "text", nullable: false),
                    BonusBenefit = table.Column<bool>(type: "boolean", nullable: false),
                    TravelBenefit = table.Column<bool>(type: "boolean", nullable: false),
                    FoodBenefit = table.Column<bool>(type: "boolean", nullable: false),
                    SalaryRange = table.Column<string>(type: "text", nullable: false),
                    PrescriptionInfo = table.Column<bool>(type: "boolean", nullable: false),
                    PrivateInsuranceEntryInfo = table.Column<bool>(type: "boolean", nullable: false),
                    OTCInfo = table.Column<bool>(type: "boolean", nullable: false),
                    DermocosmeticInfo = table.Column<bool>(type: "boolean", nullable: false),
                    OtherInfo = table.Column<string>(type: "text", nullable: true),
                    EducationStatus = table.Column<string>(type: "text", nullable: false),
                    SquareMeter = table.Column<string>(type: "text", nullable: false),
                    MonthlyTurnover = table.Column<int>(type: "integer", nullable: false),
                    LicenseRightLeft = table.Column<bool>(type: "boolean", nullable: false),
                    HasRightToCarry = table.Column<bool>(type: "boolean", nullable: false),
                    DriverLicenses = table.Column<List<string>>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adverts", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Adverts");
        }
    }
}
