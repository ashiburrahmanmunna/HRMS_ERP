using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GTERP.Migrations.GTRDB
{
    public partial class addprojectandDevice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HR_Emp_Devices",
                columns: table => new
                {
                    EmpDeviceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmpId = table.Column<int>(type: "int", nullable: false),
                    DeviceType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeviceModel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeviceBrand = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    dtIssue = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_Emp_Devices", x => x.EmpDeviceId);
                    table.ForeignKey(
                        name: "FK_HR_Emp_Devices_HR_Emp_Info_EmpId",
                        column: x => x.EmpId,
                        principalTable: "HR_Emp_Info",
                        principalColumn: "EmpId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HR_Emp_Projects",
                columns: table => new
                {
                    EmpProjectId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmpId = table.Column<int>(type: "int", nullable: false),
                    ProjectName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    dtStart = table.Column<DateTime>(type: "datetime2", nullable: true),
                    dtEnd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProjectRoll = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_Emp_Projects", x => x.EmpProjectId);
                    table.ForeignKey(
                        name: "FK_HR_Emp_Projects_HR_Emp_Info_EmpId",
                        column: x => x.EmpId,
                        principalTable: "HR_Emp_Info",
                        principalColumn: "EmpId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HR_Emp_Devices_EmpId",
                table: "HR_Emp_Devices",
                column: "EmpId");

            migrationBuilder.CreateIndex(
                name: "IX_HR_Emp_Projects_EmpId",
                table: "HR_Emp_Projects",
                column: "EmpId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HR_Emp_Devices");

            migrationBuilder.DropTable(
                name: "HR_Emp_Projects");
        }
    }
}
