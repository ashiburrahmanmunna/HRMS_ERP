using Microsoft.EntityFrameworkCore.Migrations;

namespace GTERP.Migrations.GTRDB
{
    public partial class Hr_Rawdata_Upload2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Hr_RawData_Upload_ComId",
                table: "Hr_RawData_Upload",
                column: "ComId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Hr_RawData_Upload_ComId",
                table: "Hr_RawData_Upload");
        }
    }
}
