using Microsoft.EntityFrameworkCore.Migrations;

namespace GTERP.Migrations.GTRDB
{
    public partial class ModifyQueryProcessQuery : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExcuteBy",
                table: "QueryProcessQueues",
                newName: "Type");

            migrationBuilder.AddColumn<string>(
                name: "ExcuteById",
                table: "QueryProcessQueues",
                type: "nvarchar(max)",
                nullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QueryProcessQueues_LoginUser_ExcuteByUserId",
                table: "QueryProcessQueues");

            migrationBuilder.DropIndex(
                name: "IX_QueryProcessQueues_ExcuteByUserId",
                table: "QueryProcessQueues");

            migrationBuilder.DropColumn(
                name: "ExcuteById",
                table: "QueryProcessQueues");

            migrationBuilder.DropColumn(
                name: "ExcuteByUserId",
                table: "QueryProcessQueues");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "QueryProcessQueues",
                newName: "ExcuteBy");
        }
    }
}
