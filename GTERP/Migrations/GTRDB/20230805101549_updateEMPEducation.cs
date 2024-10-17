using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GTERP.Migrations.GTRDB
{
    public partial class updateEMPEducation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "HR_Emp_Education",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "HR_Emp_Education",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "HR_Emp_Education",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "dtEnd",
                table: "HR_Emp_Education",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "dtStart",
                table: "HR_Emp_Education",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "HR_Emp_Education");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "HR_Emp_Education");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "HR_Emp_Education");

            migrationBuilder.DropColumn(
                name: "dtEnd",
                table: "HR_Emp_Education");

            migrationBuilder.DropColumn(
                name: "dtStart",
                table: "HR_Emp_Education");
        }
    }
}
