using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GTERP.Migrations.GTRDB
{
    public partial class AddZktFingerTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Blocklist",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    comId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    empCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    empName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isBlock = table.Column<bool>(type: "bit", nullable: false),
                    blockdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    unblockdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    remark = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blocklist", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "HR_ZKTFinger",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    comId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    empCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    empName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cardNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    empImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fingerindex1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fingerindex2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fingerindex3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fingerindex4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fingerindex5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fingerindex6 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fingerindex7 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fingerindex8 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fingerindex9 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fingerindex10 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeviceName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    totalIndex = table.Column<long>(type: "bigint", nullable: true),
                    isDelete = table.Column<bool>(type: "bit", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_ZKTFinger", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Blocklist");

            migrationBuilder.DropTable(
                name: "HR_ZKTFinger");
        }
    }
}
