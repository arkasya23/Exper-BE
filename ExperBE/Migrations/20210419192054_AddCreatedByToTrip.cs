using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ExperBE.Migrations
{
    public partial class AddCreatedByToTrip : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CreatedByUserId",
                table: "Trips",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Trips_CreatedByUserId",
                table: "Trips",
                column: "CreatedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Users_CreatedByUserId",
                table: "Trips",
                column: "CreatedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Users_CreatedByUserId",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Trips_CreatedByUserId",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Trips");
        }
    }
}
