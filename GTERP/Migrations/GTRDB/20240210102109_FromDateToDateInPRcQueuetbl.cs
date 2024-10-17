using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GTERP.Migrations.GTRDB
{
    public partial class FromDateToDateInPRcQueuetbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "RequestFrom",
                table: "QueryProcessQueues",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RequestTo",
                table: "QueryProcessQueues",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestFrom",
                table: "QueryProcessQueues");

            migrationBuilder.DropColumn(
                name: "RequestTo",
                table: "QueryProcessQueues");
        }
    }
}
