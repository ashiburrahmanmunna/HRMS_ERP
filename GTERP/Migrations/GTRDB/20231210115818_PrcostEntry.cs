using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GTERP.Migrations.GTRDB
{
    public partial class PrcostEntry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ComId",
                table: "Cat_Strength",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdateByUserId",
                table: "Cat_Strength",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Cat_Strength",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DailyProductionRules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FgDisPatch1st = table.Column<double>(type: "float", nullable: true),
                    FgDispatch2nd = table.Column<double>(type: "float", nullable: true),
                    Glycerin = table.Column<double>(type: "float", nullable: true),
                    Boo2 = table.Column<double>(type: "float", nullable: true),
                    unloading = table.Column<double>(type: "float", nullable: true),
                    OtherCost = table.Column<double>(type: "float", nullable: true),
                    ComId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    UpdateByUserId = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyProductionRules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Hr_BOFuploader",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmpCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmpName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeptId = table.Column<int>(type: "int", nullable: false),
                    DesigId = table.Column<int>(type: "int", nullable: false),
                    FgDispatch1st = table.Column<double>(type: "float", nullable: true),
                    FgDispatch2nd = table.Column<double>(type: "float", nullable: true),
                    Glycerin = table.Column<double>(type: "float", nullable: true),
                    Unloading = table.Column<double>(type: "float", nullable: true),
                    TotalEarnde = table.Column<double>(type: "float", nullable: true),
                    ComId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    UpdateByUserId = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hr_BOFuploader", x => x.id);
                    table.ForeignKey(
                        name: "FK_Hr_BOFuploader_Cat_Department_DeptId",
                        column: x => x.DeptId,
                        principalTable: "Cat_Department",
                        principalColumn: "DeptId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Hr_BOFuploader_Cat_Designation_DesigId",
                        column: x => x.DesigId,
                        principalTable: "Cat_Designation",
                        principalColumn: "DesigId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PRcostEntry",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UnitId = table.Column<int>(type: "int", nullable: false),
                    EmpName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    SafetyShoe = table.Column<double>(type: "float", nullable: true),
                    Uniform = table.Column<double>(type: "float", nullable: true),
                    ServiceComission = table.Column<double>(type: "float", nullable: true),
                    MedicalCost = table.Column<double>(type: "float", nullable: true),
                    ComId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    UpdateByUserId = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRcostEntry", x => x.id);
                    table.ForeignKey(
                        name: "FK_PRcostEntry_Cat_Unit_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Cat_Unit",
                        principalColumn: "UnitId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductionCostEntry",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Boundle = table.Column<double>(type: "float", nullable: true),
                    BoxPacketReel = table.Column<double>(type: "float", nullable: true),
                    Bags = table.Column<double>(type: "float", nullable: true),
                    Drum = table.Column<double>(type: "float", nullable: true),
                    ComId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    UpdateByUserId = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionCostEntry", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PSuploader",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmpCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmpName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeptId = table.Column<int>(type: "int", nullable: false),
                    DesigId = table.Column<int>(type: "int", nullable: false),
                    DtJoin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TotalPresent = table.Column<double>(type: "float", nullable: true),
                    TotalAbsent = table.Column<double>(type: "float", nullable: true),
                    BoxPacketReels = table.Column<double>(type: "float", nullable: true),
                    Drums = table.Column<double>(type: "float", nullable: true),
                    Bags = table.Column<double>(type: "float", nullable: true),
                    Unloads = table.Column<double>(type: "float", nullable: true),
                    GsWages = table.Column<double>(type: "float", nullable: true),
                    ComId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    UpdateByUserId = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PSuploader", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PSuploader_Cat_Department_DeptId",
                        column: x => x.DeptId,
                        principalTable: "Cat_Department",
                        principalColumn: "DeptId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PSuploader_Cat_Designation_DesigId",
                        column: x => x.DesigId,
                        principalTable: "Cat_Designation",
                        principalColumn: "DesigId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Hr_BOFuploader_DeptId",
                table: "Hr_BOFuploader",
                column: "DeptId");

            migrationBuilder.CreateIndex(
                name: "IX_Hr_BOFuploader_DesigId",
                table: "Hr_BOFuploader",
                column: "DesigId");

            migrationBuilder.CreateIndex(
                name: "IX_PRcostEntry_UnitId",
                table: "PRcostEntry",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_PSuploader_DeptId",
                table: "PSuploader",
                column: "DeptId");

            migrationBuilder.CreateIndex(
                name: "IX_PSuploader_DesigId",
                table: "PSuploader",
                column: "DesigId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailyProductionRules");

            migrationBuilder.DropTable(
                name: "Hr_BOFuploader");

            migrationBuilder.DropTable(
                name: "PRcostEntry");

            migrationBuilder.DropTable(
                name: "ProductionCostEntry");

            migrationBuilder.DropTable(
                name: "PSuploader");

            migrationBuilder.DropColumn(
                name: "ComId",
                table: "Cat_Strength");

            migrationBuilder.DropColumn(
                name: "UpdateByUserId",
                table: "Cat_Strength");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Cat_Strength");
        }
    }
}
