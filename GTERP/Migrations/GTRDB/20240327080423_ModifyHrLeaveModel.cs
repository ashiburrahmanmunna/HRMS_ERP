using Microsoft.EntityFrameworkCore.Migrations;

namespace GTERP.Migrations.GTRDB
{
    public partial class ModifyHrLeaveModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmpId",
                table: "TempLeaveEntryExcel",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDelete",
                table: "HR_Leave_Avail",
                type: "bit",
                nullable: true,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmpId",
                table: "TempLeaveEntryExcel");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDelete",
                table: "HR_Leave_Avail",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValue: false);
        }
    }
}
