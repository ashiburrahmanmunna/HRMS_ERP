using Microsoft.EntityFrameworkCore.Migrations;

namespace GTERP.Migrations.GTRDB
{
    public partial class Daily_req_entry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<float>(
            //    name: "BS",
            //    table: "TempEmpData",
            //    type: "real",
            //    nullable: false,
            //    defaultValue: 0f);

            //migrationBuilder.AddColumn<float>(
            //    name: "FA",
            //    table: "TempEmpData",
            //    type: "real",
            //    nullable: false,
            //    defaultValue: 0f);

            //migrationBuilder.AddColumn<float>(
            //    name: "HR",
            //    table: "TempEmpData",
            //    type: "real",
            //    nullable: false,
            //    defaultValue: 0f);

            //migrationBuilder.AddColumn<float>(
            //    name: "MA",
            //    table: "TempEmpData",
            //    type: "real",
            //    nullable: false,
            //    defaultValue: 0f);

            //migrationBuilder.AddColumn<float>(
            //    name: "TA",
            //    table: "TempEmpData",
            //    type: "real",
            //    nullable: false,
            //    defaultValue: 0f);

            migrationBuilder.CreateTable(
                name: "Daily_req_entry",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Comid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeptName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Job_Loc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Job_Nat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cost_head = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sup_A = table.Column<int>(type: "int", nullable: false),
                    Sup_G = table.Column<int>(type: "int", nullable: false),
                    Sup_B = table.Column<int>(type: "int", nullable: false),
                    Sup_C = table.Column<int>(type: "int", nullable: false),
                    Exc_A = table.Column<int>(type: "int", nullable: false),
                    Exc_G = table.Column<int>(type: "int", nullable: false),
                    Exc_B = table.Column<int>(type: "int", nullable: false),
                    Exc_C = table.Column<int>(type: "int", nullable: false),
                    Wor_A = table.Column<int>(type: "int", nullable: false),
                    Wor_G = table.Column<int>(type: "int", nullable: false),
                    Wor_B = table.Column<int>(type: "int", nullable: false),
                    Wor_C = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Daily_req_entry", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Daily_req_entry");

            //migrationBuilder.DropColumn(
            //    name: "BS",
            //    table: "TempEmpData");

            //migrationBuilder.DropColumn(
            //    name: "FA",
            //    table: "TempEmpData");

            //migrationBuilder.DropColumn(
            //    name: "HR",
            //    table: "TempEmpData");

            //migrationBuilder.DropColumn(
            //    name: "MA",
            //    table: "TempEmpData");

            //migrationBuilder.DropColumn(
            //    name: "TA",
            //    table: "TempEmpData");
        }
    }
}
