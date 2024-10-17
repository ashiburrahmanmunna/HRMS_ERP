using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GTERP.Migrations.GTRDB
{
    public partial class IsHRAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DtPresentLast",
                table: "HR_PF_Withdrawn");

            migrationBuilder.DropColumn(
                name: "WdType",
                table: "HR_PF_Withdrawn");

            migrationBuilder.RenameColumn(
                name: "DtSubmit",
                table: "HR_PF_Withdrawn",
                newName: "DtWithdrawn");

            migrationBuilder.RenameColumn(
                name: "DtReleased",
                table: "HR_PF_Withdrawn",
                newName: "DtInput");

            migrationBuilder.AlterColumn<string>(
                name: "EmpNameB",
                table: "HR_Emp_Info",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EmpName",
                table: "HR_Emp_Info",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<bool>(
                name: "IsHR",
                table: "HR_Emp_Info",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsHR",
                table: "Cat_MailSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsHR",
                table: "HR_Emp_Info");

            migrationBuilder.DropColumn(
                name: "IsHR",
                table: "Cat_MailSettings");

            migrationBuilder.RenameColumn(
                name: "DtWithdrawn",
                table: "HR_PF_Withdrawn",
                newName: "DtSubmit");

            migrationBuilder.RenameColumn(
                name: "DtInput",
                table: "HR_PF_Withdrawn",
                newName: "DtReleased");

            migrationBuilder.AddColumn<DateTime>(
                name: "DtPresentLast",
                table: "HR_PF_Withdrawn",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WdType",
                table: "HR_PF_Withdrawn",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EmpNameB",
                table: "HR_Emp_Info",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EmpName",
                table: "HR_Emp_Info",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);
        }
    }
}
