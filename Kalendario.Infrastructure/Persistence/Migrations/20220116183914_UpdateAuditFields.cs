using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kalendario.Infrastructure.Persistence.Migrations
{
    public partial class UpdateAuditFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "AuditEntities");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "AuditEntities");

            migrationBuilder.DropColumn(
                name: "UserCreated",
                table: "AuditEntities");

            migrationBuilder.RenameColumn(
                name: "UserModified",
                table: "AuditEntities",
                newName: "ActionUserId");

            migrationBuilder.AddColumn<DateTime>(
                name: "ActionDate",
                table: "AuditEntities",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActionDate",
                table: "AuditEntities");

            migrationBuilder.RenameColumn(
                name: "ActionUserId",
                table: "AuditEntities",
                newName: "UserModified");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "AuditEntities",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "AuditEntities",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserCreated",
                table: "AuditEntities",
                type: "text",
                nullable: true);
        }
    }
}
