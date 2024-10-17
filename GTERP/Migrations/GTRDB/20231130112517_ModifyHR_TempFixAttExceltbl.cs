using Microsoft.EntityFrameworkCore.Migrations;

namespace GTERP.Migrations.GTRDB
{
    public partial class ModifyHR_TempFixAttExceltbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "OT",
                table: "HR_TempFixAttExcel",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OtHour",
                table: "HR_TempFixAttExcel",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "HR_TempFixAttExcel",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OT",
                table: "HR_TempFixAttExcel");

            migrationBuilder.DropColumn(
                name: "OtHour",
                table: "HR_TempFixAttExcel");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "HR_TempFixAttExcel");
        }
    }
}
