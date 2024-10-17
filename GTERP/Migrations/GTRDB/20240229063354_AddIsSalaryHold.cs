using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GTERP.Migrations.GTRDB
{
    public partial class AddIsSalaryHold : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSalaryHold",
                table: "HR_Emp_Info",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CompanyCode",
                table: "HR_Emp_Increment",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FingerId",
                table: "HR_Emp_Increment",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NewShiftId",
                table: "HR_Emp_Increment",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "New_EmpCode",
                table: "HR_Emp_Increment",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OldShiftId",
                table: "HR_Emp_Increment",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TransferDate",
                table: "HR_Emp_Increment",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HR_Emp_Increment_NewShiftId",
                table: "HR_Emp_Increment",
                column: "NewShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_HR_Emp_Increment_OldShiftId",
                table: "HR_Emp_Increment",
                column: "OldShiftId");

            migrationBuilder.AddForeignKey(
                name: "FK_HR_Emp_Increment_Cat_Shift_NewShiftId",
                table: "HR_Emp_Increment",
                column: "NewShiftId",
                principalTable: "Cat_Shift",
                principalColumn: "ShiftId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HR_Emp_Increment_Cat_Shift_OldShiftId",
                table: "HR_Emp_Increment",
                column: "OldShiftId",
                principalTable: "Cat_Shift",
                principalColumn: "ShiftId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HR_Emp_Increment_Cat_Shift_NewShiftId",
                table: "HR_Emp_Increment");

            migrationBuilder.DropForeignKey(
                name: "FK_HR_Emp_Increment_Cat_Shift_OldShiftId",
                table: "HR_Emp_Increment");

            migrationBuilder.DropIndex(
                name: "IX_HR_Emp_Increment_NewShiftId",
                table: "HR_Emp_Increment");

            migrationBuilder.DropIndex(
                name: "IX_HR_Emp_Increment_OldShiftId",
                table: "HR_Emp_Increment");

            migrationBuilder.DropColumn(
                name: "IsSalaryHold",
                table: "HR_Emp_Info");

            migrationBuilder.DropColumn(
                name: "CompanyCode",
                table: "HR_Emp_Increment");

            migrationBuilder.DropColumn(
                name: "FingerId",
                table: "HR_Emp_Increment");

            migrationBuilder.DropColumn(
                name: "NewShiftId",
                table: "HR_Emp_Increment");

            migrationBuilder.DropColumn(
                name: "New_EmpCode",
                table: "HR_Emp_Increment");

            migrationBuilder.DropColumn(
                name: "OldShiftId",
                table: "HR_Emp_Increment");

            migrationBuilder.DropColumn(
                name: "TransferDate",
                table: "HR_Emp_Increment");
        }
    }
}
