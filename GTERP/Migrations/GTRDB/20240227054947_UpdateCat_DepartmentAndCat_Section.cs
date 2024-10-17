using Microsoft.EntityFrameworkCore.Migrations;

namespace GTERP.Migrations.GTRDB
{
    public partial class UpdateCat_DepartmentAndCat_Section : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActualOTHide",
                table: "Cat_Section",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActualSalaryHide",
                table: "Cat_Section",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsBuyerOTDiffer",
                table: "Cat_Section",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsBuyerOTHide",
                table: "Cat_Section",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsBuyerSalaryHide",
                table: "Cat_Section",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActualOTHide",
                table: "Cat_Department",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActualSalaryHide",
                table: "Cat_Department",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsBuyerOTDiffer",
                table: "Cat_Department",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsBuyerOTHide",
                table: "Cat_Department",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsBuyerSalaryHide",
                table: "Cat_Department",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActualOTHide",
                table: "Cat_Section");

            migrationBuilder.DropColumn(
                name: "IsActualSalaryHide",
                table: "Cat_Section");

            migrationBuilder.DropColumn(
                name: "IsBuyerOTDiffer",
                table: "Cat_Section");

            migrationBuilder.DropColumn(
                name: "IsBuyerOTHide",
                table: "Cat_Section");

            migrationBuilder.DropColumn(
                name: "IsBuyerSalaryHide",
                table: "Cat_Section");

            migrationBuilder.DropColumn(
                name: "IsActualOTHide",
                table: "Cat_Department");

            migrationBuilder.DropColumn(
                name: "IsActualSalaryHide",
                table: "Cat_Department");

            migrationBuilder.DropColumn(
                name: "IsBuyerOTDiffer",
                table: "Cat_Department");

            migrationBuilder.DropColumn(
                name: "IsBuyerOTHide",
                table: "Cat_Department");

            migrationBuilder.DropColumn(
                name: "IsBuyerSalaryHide",
                table: "Cat_Department");
        }
    }
}
