using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GTERP.Migrations.GTRDB
{
    public partial class AddFixedAttB_ProsDataB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HR_AttFixedB",
                columns: table => new
                {
                    AttId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComId = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    EmpId = table.Column<int>(type: "int", nullable: false),
                    DtPunchDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ShiftId = table.Column<int>(type: "int", nullable: false),
                    TimeIn = table.Column<TimeSpan>(type: "time", nullable: false),
                    TimeOut = table.Column<TimeSpan>(type: "time", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OTHour = table.Column<float>(type: "real", nullable: false),
                    OTHourInTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    OT = table.Column<float>(type: "real", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsInactive = table.Column<bool>(type: "bit", nullable: false),
                    TimeInPrev = table.Column<TimeSpan>(type: "time", nullable: false),
                    TimeOutPrev = table.Column<TimeSpan>(type: "time", nullable: false),
                    StatusPrev = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OTHourPrev = table.Column<float>(type: "real", nullable: false),
                    PcName = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    DtTran = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsApprove = table.Column<bool>(type: "bit", nullable: false),
                    ApprovedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_AttFixedB", x => x.AttId);
                    table.ForeignKey(
                        name: "FK_HR_AttFixedB_HR_Emp_Info_EmpId",
                        column: x => x.EmpId,
                        principalTable: "HR_Emp_Info",
                        principalColumn: "EmpId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HR_ProcessedDataB",
                columns: table => new
                {
                    PId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComId = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    EmpId = table.Column<int>(type: "int", nullable: false),
                    EmpCode = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    DtPunchDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ShiftId = table.Column<int>(type: "int", nullable: false),
                    DeptId = table.Column<int>(type: "int", nullable: false),
                    SectId = table.Column<int>(type: "int", nullable: false),
                    TimeIn = table.Column<TimeSpan>(type: "time", nullable: false),
                    TimeOut = table.Column<TimeSpan>(type: "time", nullable: false),
                    Late = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    RegHour = table.Column<float>(type: "real", nullable: false),
                    OTHour = table.Column<float>(type: "real", nullable: false),
                    OT = table.Column<float>(type: "real", nullable: true),
                    OTHourDed = table.Column<float>(type: "real", nullable: false),
                    ROT = table.Column<float>(type: "real", nullable: false),
                    EOT = table.Column<float>(type: "real", nullable: false),
                    StaffOT = table.Column<float>(type: "real", nullable: false),
                    IsLunchDay = table.Column<float>(type: "real", nullable: false),
                    IsNightShift = table.Column<float>(type: "real", nullable: false),
                    ShiftIn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AdJusted = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_ProcessedDataB", x => x.PId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HR_AttFixedB_EmpId",
                table: "HR_AttFixedB",
                column: "EmpId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HR_AttFixedB");

            migrationBuilder.DropTable(
                name: "HR_ProcessedDataB");
        }
    }
}
