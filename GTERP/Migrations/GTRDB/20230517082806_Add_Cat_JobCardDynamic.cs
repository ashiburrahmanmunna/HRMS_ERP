using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GTERP.Migrations.GTRDB
{
    public partial class Add_Cat_JobCardDynamic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.CreateTable(
                name: "Cat_JobCardDynamic",
                columns: table => new
                {
                    JcdId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    dtFrom = table.Column<DateTime>(type: "datetime2", nullable: true),
                    dtTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OTHrLimit = table.Column<float>(type: "real", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsHide = table.Column<bool>(type: "bit", nullable: false),
                    WHDayOT = table.Column<float>(type: "real", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    userId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dateupdated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cat_JobCardDynamic", x => x.JcdId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cat_JobCardDynamic");

            migrationBuilder.DropColumn(
                name: "suid",
                table: "HR_ProcessedDataSalUpdate");

            migrationBuilder.RenameTable(
                name: "HR_ProcessedDataSalUpdate",
                newName: "HR_ProcessedDataSalUpdates");

            migrationBuilder.RenameTable(
                name: "HR_ProcessedDataSal",
                newName: "HR_ProcessedDataSals");
        }
    }
}
