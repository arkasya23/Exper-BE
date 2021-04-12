using Microsoft.EntityFrameworkCore.Migrations;

namespace ExperBE.Migrations
{
    public partial class CascadeOnGroupExpenseDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupExpenseUsers_GroupExpenses_GroupExpenseId",
                table: "GroupExpenseUsers");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupExpenseUsers_GroupExpenses_GroupExpenseId",
                table: "GroupExpenseUsers",
                column: "GroupExpenseId",
                principalTable: "GroupExpenses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupExpenseUsers_GroupExpenses_GroupExpenseId",
                table: "GroupExpenseUsers");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupExpenseUsers_GroupExpenses_GroupExpenseId",
                table: "GroupExpenseUsers",
                column: "GroupExpenseId",
                principalTable: "GroupExpenses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
