using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GTERP.Migrations.GTRDB
{
    public partial class HR_Employee_TransferAddedAndDbUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NewFloorId",
                table: "HR_Emp_Increment",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NewLineId",
                table: "HR_Emp_Increment",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OldFloorId",
                table: "HR_Emp_Increment",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OldLineId",
                table: "HR_Emp_Increment",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "HR_Employee_Transfer",
                columns: table => new
                {
                    TransferId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OldComId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewComId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmpId = table.Column<int>(type: "int", nullable: false),
                    NewEmpCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmpName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewUnitId = table.Column<int>(type: "int", nullable: true),
                    NewDeptId = table.Column<int>(type: "int", nullable: true),
                    NewSectId = table.Column<int>(type: "int", nullable: true),
                    NewDesigId = table.Column<int>(type: "int", nullable: true),
                    NewShiftId = table.Column<int>(type: "int", nullable: true),
                    NewEmpTypeId = table.Column<int>(type: "int", nullable: true),
                    NewFloorId = table.Column<int>(type: "int", nullable: true),
                    NewLineId = table.Column<int>(type: "int", nullable: true),
                    FingerId = table.Column<int>(type: "int", nullable: true),
                    TransferDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_Employee_Transfer", x => x.TransferId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HR_Emp_Increment_NewFloorId",
                table: "HR_Emp_Increment",
                column: "NewFloorId");

            migrationBuilder.CreateIndex(
                name: "IX_HR_Emp_Increment_NewLineId",
                table: "HR_Emp_Increment",
                column: "NewLineId");

            migrationBuilder.CreateIndex(
                name: "IX_HR_Emp_Increment_OldFloorId",
                table: "HR_Emp_Increment",
                column: "OldFloorId");

            migrationBuilder.CreateIndex(
                name: "IX_HR_Emp_Increment_OldLineId",
                table: "HR_Emp_Increment",
                column: "OldLineId");

            migrationBuilder.AddForeignKey(
                name: "FK_HR_Emp_Increment_Cat_Floor_NewFloorId",
                table: "HR_Emp_Increment",
                column: "NewFloorId",
                principalTable: "Cat_Floor",
                principalColumn: "FloorId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HR_Emp_Increment_Cat_Floor_OldFloorId",
                table: "HR_Emp_Increment",
                column: "OldFloorId",
                principalTable: "Cat_Floor",
                principalColumn: "FloorId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HR_Emp_Increment_Cat_Line_NewLineId",
                table: "HR_Emp_Increment",
                column: "NewLineId",
                principalTable: "Cat_Line",
                principalColumn: "LineId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HR_Emp_Increment_Cat_Line_OldLineId",
                table: "HR_Emp_Increment",
                column: "OldLineId",
                principalTable: "Cat_Line",
                principalColumn: "LineId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HR_Emp_Increment_Cat_Floor_NewFloorId",
                table: "HR_Emp_Increment");

            migrationBuilder.DropForeignKey(
                name: "FK_HR_Emp_Increment_Cat_Floor_OldFloorId",
                table: "HR_Emp_Increment");

            migrationBuilder.DropForeignKey(
                name: "FK_HR_Emp_Increment_Cat_Line_NewLineId",
                table: "HR_Emp_Increment");

            migrationBuilder.DropForeignKey(
                name: "FK_HR_Emp_Increment_Cat_Line_OldLineId",
                table: "HR_Emp_Increment");

            migrationBuilder.DropTable(
                name: "HR_Employee_Transfer");

            migrationBuilder.DropIndex(
                name: "IX_HR_Emp_Increment_NewFloorId",
                table: "HR_Emp_Increment");

            migrationBuilder.DropIndex(
                name: "IX_HR_Emp_Increment_NewLineId",
                table: "HR_Emp_Increment");

            migrationBuilder.DropIndex(
                name: "IX_HR_Emp_Increment_OldFloorId",
                table: "HR_Emp_Increment");

            migrationBuilder.DropIndex(
                name: "IX_HR_Emp_Increment_OldLineId",
                table: "HR_Emp_Increment");

            migrationBuilder.DropColumn(
                name: "NewFloorId",
                table: "HR_Emp_Increment");

            migrationBuilder.DropColumn(
                name: "NewLineId",
                table: "HR_Emp_Increment");

            migrationBuilder.DropColumn(
                name: "OldFloorId",
                table: "HR_Emp_Increment");

            migrationBuilder.DropColumn(
                name: "OldLineId",
                table: "HR_Emp_Increment");
        }
    }
}
