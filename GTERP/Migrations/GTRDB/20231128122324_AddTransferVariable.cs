using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GTERP.Migrations.GTRDB
{
    public partial class AddTransferVariable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalHour",
                table: "HR_Leave_Avail",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TransferVariable",
                columns: table => new
                {
                    TVId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EffectiveDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SINo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ComId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmpId = table.Column<int>(type: "int", nullable: false),
                    EmpCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VandorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PresentDepartment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProposedDepartment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PresentDesignation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProposedDesignation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PresentRole = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProposedRole = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PresentCostHead = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProposedCostHead = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PresentAltitudeCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProposedAltitudeCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PresentWorkerClassification = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProposedClassification = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    UpdateByUserId = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferVariable", x => x.TVId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransferVariable");

            migrationBuilder.DropColumn(
                name: "TotalHour",
                table: "HR_Leave_Avail");
        }
    }
}
