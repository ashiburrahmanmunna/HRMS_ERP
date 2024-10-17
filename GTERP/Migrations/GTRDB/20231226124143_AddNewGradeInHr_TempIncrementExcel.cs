using Microsoft.EntityFrameworkCore.Migrations;

namespace GTERP.Migrations.GTRDB
{
    public partial class AddNewGradeInHr_TempIncrementExcel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NewGradeName",
                table: "HR_TempIncrementExcel",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HR_Emp_Increment_NewGradeId",
                table: "HR_Emp_Increment",
                column: "NewGradeId");

            migrationBuilder.CreateIndex(
                name: "IX_HR_Emp_Increment_OldGradeId",
                table: "HR_Emp_Increment",
                column: "OldGradeId");

            migrationBuilder.AddForeignKey(
                name: "FK_HR_Emp_Increment_Cat_Grade_NewGradeId",
                table: "HR_Emp_Increment",
                column: "NewGradeId",
                principalTable: "Cat_Grade",
                principalColumn: "GradeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HR_Emp_Increment_Cat_Grade_OldGradeId",
                table: "HR_Emp_Increment",
                column: "OldGradeId",
                principalTable: "Cat_Grade",
                principalColumn: "GradeId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HR_Emp_Increment_Cat_Grade_NewGradeId",
                table: "HR_Emp_Increment");

            migrationBuilder.DropForeignKey(
                name: "FK_HR_Emp_Increment_Cat_Grade_OldGradeId",
                table: "HR_Emp_Increment");

            migrationBuilder.DropIndex(
                name: "IX_HR_Emp_Increment_NewGradeId",
                table: "HR_Emp_Increment");

            migrationBuilder.DropIndex(
                name: "IX_HR_Emp_Increment_OldGradeId",
                table: "HR_Emp_Increment");

            migrationBuilder.DropColumn(
                name: "NewGradeName",
                table: "HR_TempIncrementExcel");
        }
    }
}
