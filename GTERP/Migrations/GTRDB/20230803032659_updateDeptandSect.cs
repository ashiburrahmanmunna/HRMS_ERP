using Microsoft.EntityFrameworkCore.Migrations;

namespace GTERP.Migrations.GTRDB
{
    public partial class updateDeptandSect : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SectCLevelId",
                table: "Cat_Section",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SectHODId",
                table: "Cat_Section",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeptCLevelId",
                table: "Cat_Department",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeptHODId",
                table: "Cat_Department",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SectCLevelId",
                table: "Cat_Section");

            migrationBuilder.DropColumn(
                name: "SectHODId",
                table: "Cat_Section");

            migrationBuilder.DropColumn(
                name: "DeptCLevelId",
                table: "Cat_Department");

            migrationBuilder.DropColumn(
                name: "DeptHODId",
                table: "Cat_Department");
        }
    }
}
