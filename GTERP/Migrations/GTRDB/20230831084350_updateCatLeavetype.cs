using Microsoft.EntityFrameworkCore.Migrations;

namespace GTERP.Migrations.GTRDB
{
    public partial class updateCatLeavetype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAllowHalfLeave",
                table: "Cat_Leave_Type",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "Cat_Leave_Type",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsInActive",
                table: "Cat_Leave_Type",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsValidation",
                table: "Cat_Leave_Type",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ValidDays",
                table: "Cat_Leave_Type",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAllowHalfLeave",
                table: "Cat_Leave_Type");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "Cat_Leave_Type");

            migrationBuilder.DropColumn(
                name: "IsInActive",
                table: "Cat_Leave_Type");

            migrationBuilder.DropColumn(
                name: "IsValidation",
                table: "Cat_Leave_Type");

            migrationBuilder.DropColumn(
                name: "ValidDays",
                table: "Cat_Leave_Type");
        }
    }
}
