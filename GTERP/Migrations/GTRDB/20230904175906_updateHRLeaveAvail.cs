using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GTERP.Migrations.GTRDB
{
    public partial class updateHRLeaveAvail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LeaveOption",
                table: "HR_Leave_Avail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "dtWork",
                table: "HR_Leave_Avail",
                type: "date",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LeaveOption",
                table: "HR_Leave_Avail");

            migrationBuilder.DropColumn(
                name: "dtWork",
                table: "HR_Leave_Avail");
        }
    }
}
