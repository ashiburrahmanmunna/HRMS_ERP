using Microsoft.EntityFrameworkCore.Migrations;

namespace GTERP.Migrations.GTRDB
{
    public partial class addIsDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WHDayOT",
                table: "Cat_JobCardDynamic");

            migrationBuilder.RenameColumn(
                name: "IsHide",
                table: "Cat_JobCardDynamic",
                newName: "IsHideWHDayOT");

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "Cat_JobCardDynamic",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "Cat_JobCardDynamic");

            migrationBuilder.RenameColumn(
                name: "IsHideWHDayOT",
                table: "Cat_JobCardDynamic",
                newName: "IsHide");

            migrationBuilder.AddColumn<float>(
                name: "WHDayOT",
                table: "Cat_JobCardDynamic",
                type: "real",
                nullable: true);
        }
    }
}
