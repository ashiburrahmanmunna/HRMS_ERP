using Microsoft.EntityFrameworkCore.Migrations;

namespace GTERP.Migrations.GTRDB
{
    public partial class ModifyQueryProcessQueryUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QueryProcessQueues_LoginUser_ExcuteByUserId",
                table: "QueryProcessQueues");

            migrationBuilder.DropIndex(
                name: "IX_QueryProcessQueues_ExcuteByUserId",
                table: "QueryProcessQueues");

            migrationBuilder.DropColumn(
                name: "ExcuteByUserId",
                table: "QueryProcessQueues");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExcuteByUserId",
                table: "QueryProcessQueues",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_QueryProcessQueues_ExcuteByUserId",
                table: "QueryProcessQueues",
                column: "ExcuteByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_QueryProcessQueues_LoginUser_ExcuteByUserId",
                table: "QueryProcessQueues",
                column: "ExcuteByUserId",
                principalTable: "LoginUser",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
