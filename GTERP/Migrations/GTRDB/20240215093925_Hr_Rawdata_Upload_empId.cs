using Microsoft.EntityFrameworkCore.Migrations;

namespace GTERP.Migrations.GTRDB
{
    public partial class Hr_Rawdata_Upload_empId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmpId",
                table: "Hr_RawData_Upload",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Hr_RawData_Upload_EmpId",
                table: "Hr_RawData_Upload",
                column: "EmpId");

            migrationBuilder.AddForeignKey(
                name: "FK_Hr_RawData_Upload_HR_Emp_Info_EmpId",
                table: "Hr_RawData_Upload",
                column: "EmpId",
                principalTable: "HR_Emp_Info",
                principalColumn: "EmpId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hr_RawData_Upload_HR_Emp_Info_EmpId",
                table: "Hr_RawData_Upload");

            migrationBuilder.DropIndex(
                name: "IX_Hr_RawData_Upload_EmpId",
                table: "Hr_RawData_Upload");

            migrationBuilder.DropColumn(
                name: "EmpId",
                table: "Hr_RawData_Upload");
        }
    }
}
