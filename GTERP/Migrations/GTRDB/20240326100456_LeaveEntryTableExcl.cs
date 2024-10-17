using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GTERP.Migrations.GTRDB
{
    public partial class LeaveEntryTableExcl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "HR_Leave_Avail",
                type: "bit",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TempLeaveEntryExcel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmpCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DtInput = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DtFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DtTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LvType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalDay = table.Column<int>(type: "int", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TempLeaveEntryExcel", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TempLeaveEntryExcel");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "HR_Leave_Avail");
        }
    }
}
