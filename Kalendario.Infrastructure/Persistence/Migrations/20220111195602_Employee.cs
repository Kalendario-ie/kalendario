using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kalendario.Infrastructure.Persistence.Migrations
{
    public partial class Employee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Services_ServiceId1",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_ServiceId1",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "ServiceId1",
                table: "Appointments");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Employees",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Employees",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ScheduleId",
                table: "Employees",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ScheduleId",
                table: "Employees",
                column: "ScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Schedules_ScheduleId",
                table: "Employees",
                column: "ScheduleId",
                principalTable: "Schedules",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Schedules_ScheduleId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_ScheduleId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "ScheduleId",
                table: "Employees");

            migrationBuilder.AddColumn<Guid>(
                name: "ServiceId1",
                table: "Appointments",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_ServiceId1",
                table: "Appointments",
                column: "ServiceId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Services_ServiceId1",
                table: "Appointments",
                column: "ServiceId1",
                principalTable: "Services",
                principalColumn: "Id");
        }
    }
}
