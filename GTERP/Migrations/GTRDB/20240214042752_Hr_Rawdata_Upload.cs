using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GTERP.Migrations.GTRDB
{
    public partial class Hr_Rawdata_Upload : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Hr_RawData_Upload",
                columns: table => new
                {
                    aId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DeviceNo = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    CardNo = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    DtPunchDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DtPunchTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hr_RawData_Upload", x => x.aId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Hr_RawData_Upload");
        }
    }
}
