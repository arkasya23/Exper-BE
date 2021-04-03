using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ExperBE.Migrations
{
    public partial class AllEntitiesWithProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Trips",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Trips",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "PersonalExpenses",
                type: "decimal(19,4)",
                precision: 19,
                scale: 4,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "PersonalExpenses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "PersonalExpenses",
                type: "nvarchar(2047)",
                maxLength: 2047,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "TripId",
                table: "PersonalExpenses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Notifications",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Notifications",
                type: "nvarchar(2047)",
                maxLength: 2047,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Notifications",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Notifications",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "GroupExpenseId",
                table: "GroupExpenseUsers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "GroupExpenseUsers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "GroupExpenses",
                type: "decimal(19,4)",
                precision: 19,
                scale: 4,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "GroupExpenses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "GroupExpenses",
                type: "nvarchar(2047)",
                maxLength: 2047,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "DivideBetweenAllMembers",
                table: "GroupExpenses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "TripId",
                table: "GroupExpenses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_PersonalExpenses_CreatedById",
                table: "PersonalExpenses",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalExpenses_TripId",
                table: "PersonalExpenses",
                column: "TripId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupExpenseUsers_GroupExpenseId_UserId",
                table: "GroupExpenseUsers",
                columns: new[] { "GroupExpenseId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroupExpenseUsers_UserId",
                table: "GroupExpenseUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupExpenses_CreatedById",
                table: "GroupExpenses",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_GroupExpenses_TripId",
                table: "GroupExpenses",
                column: "TripId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupExpenses_Trips_TripId",
                table: "GroupExpenses",
                column: "TripId",
                principalTable: "Trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupExpenses_Users_CreatedById",
                table: "GroupExpenses",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupExpenseUsers_GroupExpenses_GroupExpenseId",
                table: "GroupExpenseUsers",
                column: "GroupExpenseId",
                principalTable: "GroupExpenses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupExpenseUsers_Users_UserId",
                table: "GroupExpenseUsers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Users_UserId",
                table: "Notifications",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalExpenses_Trips_TripId",
                table: "PersonalExpenses",
                column: "TripId",
                principalTable: "Trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalExpenses_Users_CreatedById",
                table: "PersonalExpenses",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupExpenses_Trips_TripId",
                table: "GroupExpenses");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupExpenses_Users_CreatedById",
                table: "GroupExpenses");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupExpenseUsers_GroupExpenses_GroupExpenseId",
                table: "GroupExpenseUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupExpenseUsers_Users_UserId",
                table: "GroupExpenseUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Users_UserId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonalExpenses_Trips_TripId",
                table: "PersonalExpenses");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonalExpenses_Users_CreatedById",
                table: "PersonalExpenses");

            migrationBuilder.DropIndex(
                name: "IX_PersonalExpenses_CreatedById",
                table: "PersonalExpenses");

            migrationBuilder.DropIndex(
                name: "IX_PersonalExpenses_TripId",
                table: "PersonalExpenses");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_GroupExpenseUsers_GroupExpenseId_UserId",
                table: "GroupExpenseUsers");

            migrationBuilder.DropIndex(
                name: "IX_GroupExpenseUsers_UserId",
                table: "GroupExpenseUsers");

            migrationBuilder.DropIndex(
                name: "IX_GroupExpenses_CreatedById",
                table: "GroupExpenses");

            migrationBuilder.DropIndex(
                name: "IX_GroupExpenses_TripId",
                table: "GroupExpenses");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "PersonalExpenses");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "PersonalExpenses");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "PersonalExpenses");

            migrationBuilder.DropColumn(
                name: "TripId",
                table: "PersonalExpenses");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "GroupExpenseId",
                table: "GroupExpenseUsers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "GroupExpenseUsers");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "GroupExpenses");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "GroupExpenses");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "GroupExpenses");

            migrationBuilder.DropColumn(
                name: "DivideBetweenAllMembers",
                table: "GroupExpenses");

            migrationBuilder.DropColumn(
                name: "TripId",
                table: "GroupExpenses");
        }
    }
}
