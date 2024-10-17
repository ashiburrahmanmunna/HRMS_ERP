using Microsoft.EntityFrameworkCore.Migrations;

namespace GTERP.Migrations.GTRDB
{
    public partial class ModifyHrInfotbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OldGradeId",
                table: "HR_Emp_Info",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NewGradeId",
                table: "HR_Emp_Increment",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OldGradeId",
                table: "HR_Emp_Increment",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OldGradeId",
                table: "HR_Emp_Info");

            migrationBuilder.DropColumn(
                name: "NewGradeId",
                table: "HR_Emp_Increment");

            migrationBuilder.DropColumn(
                name: "OldGradeId",
                table: "HR_Emp_Increment");
        }
    }
}
