using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kalendario.Infrastructure.Persistence.Migrations
{
    public partial class AddServiceCategoriesToDbContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceCategory_Accounts_AccountId",
                table: "ServiceCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_ServiceCategory_ServiceCategoryId",
                table: "Services");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ServiceCategory",
                table: "ServiceCategory");

            migrationBuilder.RenameTable(
                name: "ServiceCategory",
                newName: "ServiceCategories");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceCategory_AccountId",
                table: "ServiceCategories",
                newName: "IX_ServiceCategories_AccountId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServiceCategories",
                table: "ServiceCategories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceCategories_Accounts_AccountId",
                table: "ServiceCategories",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Services_ServiceCategories_ServiceCategoryId",
                table: "Services",
                column: "ServiceCategoryId",
                principalTable: "ServiceCategories",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceCategories_Accounts_AccountId",
                table: "ServiceCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_ServiceCategories_ServiceCategoryId",
                table: "Services");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ServiceCategories",
                table: "ServiceCategories");

            migrationBuilder.RenameTable(
                name: "ServiceCategories",
                newName: "ServiceCategory");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceCategories_AccountId",
                table: "ServiceCategory",
                newName: "IX_ServiceCategory_AccountId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServiceCategory",
                table: "ServiceCategory",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceCategory_Accounts_AccountId",
                table: "ServiceCategory",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Services_ServiceCategory_ServiceCategoryId",
                table: "Services",
                column: "ServiceCategoryId",
                principalTable: "ServiceCategory",
                principalColumn: "Id");
        }
    }
}
