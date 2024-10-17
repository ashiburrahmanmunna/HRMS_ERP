using Microsoft.EntityFrameworkCore.Migrations;

namespace GTERP.Migrations.GTRDB
{
    public partial class addstatusINHREmpInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "HR_Emp_BankInfo",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "HR_Emp_BankInfo");
        }
    }
}
