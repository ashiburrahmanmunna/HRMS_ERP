using Microsoft.EntityFrameworkCore.Migrations;

namespace GTERP.Migrations.GTRDB
{
    public partial class updateHREMPINFO : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FinalAprvId",
                table: "HR_Emp_Info",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FirstAprvId",
                table: "HR_Emp_Info",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsHOD",
                table: "HR_Emp_Info",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinalAprvId",
                table: "HR_Emp_Info");

            migrationBuilder.DropColumn(
                name: "FirstAprvId",
                table: "HR_Emp_Info");

            migrationBuilder.DropColumn(
                name: "IsHOD",
                table: "HR_Emp_Info");
        }
    }
}
