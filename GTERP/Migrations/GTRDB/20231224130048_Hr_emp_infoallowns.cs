using Microsoft.EntityFrameworkCore.Migrations;

namespace GTERP.Migrations.GTRDB
{
    public partial class Hr_emp_infoallowns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsLunchAllow",
                table: "HR_Emp_Info",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsLunchDed",
                table: "HR_Emp_Info",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsTrnAllow",
                table: "HR_Emp_Info",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsTrnDed",
                table: "HR_Emp_Info",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsLunchAllow",
                table: "HR_Emp_Info");

            migrationBuilder.DropColumn(
                name: "IsLunchDed",
                table: "HR_Emp_Info");

            migrationBuilder.DropColumn(
                name: "IsTrnAllow",
                table: "HR_Emp_Info");

            migrationBuilder.DropColumn(
                name: "IsTrnDed",
                table: "HR_Emp_Info");
        }
    }
}
