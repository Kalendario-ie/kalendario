using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kalendario.Persistence.Migrations
{
    public partial class AddEmployeeServiceAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AccountId",
                table: "EmployeeService",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "EmployeeService",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "EmployeeService",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserCreated",
                table: "EmployeeService",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserModified",
                table: "EmployeeService",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeService_AccountId",
                table: "EmployeeService",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeService_Accounts_AccountId",
                table: "EmployeeService",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeService_Accounts_AccountId",
                table: "EmployeeService");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeService_AccountId",
                table: "EmployeeService");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "EmployeeService");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "EmployeeService");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "EmployeeService");

            migrationBuilder.DropColumn(
                name: "UserCreated",
                table: "EmployeeService");

            migrationBuilder.DropColumn(
                name: "UserModified",
                table: "EmployeeService");
        }
    }
}
