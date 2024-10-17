using Microsoft.EntityFrameworkCore.Migrations;

namespace GTERP.Migrations.GTRDB
{
    public partial class updateOvertime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsManualOT",
                table: "HR_OverTimeSetting",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "OTRate",
                table: "HR_OverTimeSetting",
                type: "real",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsManualOT",
                table: "HR_OverTimeSetting");

            migrationBuilder.DropColumn(
                name: "OTRate",
                table: "HR_OverTimeSetting");
        }
    }
}
