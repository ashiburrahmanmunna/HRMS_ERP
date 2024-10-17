using Microsoft.EntityFrameworkCore.Migrations;

namespace GTERP.Migrations.GTRDB
{
    public partial class Hr_Rawdata_Upload_deletetrace : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrackId",
                table: "Hr_RawData_Upload");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TrackId",
                table: "Hr_RawData_Upload",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
