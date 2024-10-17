using Microsoft.EntityFrameworkCore.Migrations;

namespace GTERP.Migrations.GTRDB
{
    public partial class modifyQueryProcessQueue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExcuteBy",
                table: "QueryProcessQueues",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IsExecuted",
                table: "QueryProcessQueues",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExcuteBy",
                table: "QueryProcessQueues");

            migrationBuilder.DropColumn(
                name: "IsExecuted",
                table: "QueryProcessQueues");
        }
    }
}
