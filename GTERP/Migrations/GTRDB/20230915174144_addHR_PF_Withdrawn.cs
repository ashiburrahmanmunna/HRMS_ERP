using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GTERP.Migrations.GTRDB
{
    public partial class addHR_PF_Withdrawn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HR_PF_Withdrawn",
                columns: table => new
                {
                    WdId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmpId = table.Column<int>(type: "int", nullable: false),
                    DtReleased = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    WdType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    DtPresentLast = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DtSubmit = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PcName = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    IsApprove = table.Column<bool>(type: "bit", nullable: false),
                    ApprovedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ComId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    UpdateByUserId = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_PF_Withdrawn", x => x.WdId);
                    table.ForeignKey(
                        name: "FK_HR_PF_Withdrawn_HR_Emp_Info_EmpId",
                        column: x => x.EmpId,
                        principalTable: "HR_Emp_Info",
                        principalColumn: "EmpId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HR_PF_Withdrawn_EmpId",
                table: "HR_PF_Withdrawn",
                column: "EmpId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HR_PF_Withdrawn");
        }
    }
}
