using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GTERP.Migrations.GTRDB
{
    public partial class updateFiscalYearId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FiscalYearId",
                table: "Tax_ClientContactPayment",
                type: "nvarchar(20)",
                nullable: true);

            //migrationBuilder.CreateTable(
            //    name: "PF_FiscalMonth",
            //    columns: table => new
            //    {
            //        FiscalMonthId = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        MonthId = table.Column<int>(type: "int", nullable: false),
            //        MonthName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        MonthNameBangla = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        dtFrom = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        dtTo = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        OpeningdtFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        ClosingdtTo = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        aid = table.Column<int>(type: "int", nullable: false),
            //        FYId = table.Column<int>(type: "int", nullable: false),
            //        HYearId = table.Column<int>(type: "int", nullable: false),
            //        QtrId = table.Column<int>(type: "int", nullable: false),
            //        ComId = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
            //        isLocked = table.Column<bool>(type: "bit", nullable: false),
            //        isLockedStore = table.Column<bool>(type: "bit", nullable: false),
            //        isLockedAccounts = table.Column<bool>(type: "bit", nullable: false),
            //        isLockedAttendance = table.Column<bool>(type: "bit", nullable: false),
            //        isLockedSalary = table.Column<bool>(type: "bit", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_PF_FiscalMonth", x => x.FiscalMonthId);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "PF_FiscalYear",
            //    columns: table => new
            //    {
            //        FiscalYearId = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        FYId = table.Column<int>(type: "int", nullable: false),
            //        FYName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        FYNameBangla = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        OpDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ClDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        OpeningDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        ClosingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        isWorking = table.Column<bool>(type: "bit", nullable: false),
            //        isRunning = table.Column<bool>(type: "bit", nullable: false),
            //        RowNo = table.Column<int>(type: "int", nullable: true),
            //        isLocked = table.Column<bool>(type: "bit", nullable: false),
            //        ComId = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        UserId = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
            //        UpdateByUserId = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
            //        IsDelete = table.Column<bool>(type: "bit", nullable: false),
            //        DateAdded = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_PF_FiscalYear", x => x.FiscalYearId);
            //    });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PF_FiscalMonth");

            migrationBuilder.DropTable(
                name: "PF_FiscalYear");

            migrationBuilder.DropColumn(
                name: "FiscalYearId",
                table: "Tax_ClientContactPayment");
        }
    }
}
