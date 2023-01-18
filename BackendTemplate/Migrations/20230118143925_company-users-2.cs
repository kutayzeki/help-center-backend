using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FeedbackHub.Migrations
{
    /// <inheritdoc />
    public partial class companyusers2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyUsers_AspNetRoles_RoleId1",
                table: "CompanyUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CompanyUsers",
                table: "CompanyUsers");

            migrationBuilder.DropIndex(
                name: "IX_CompanyUsers_RoleId1",
                table: "CompanyUsers");

            migrationBuilder.DropIndex(
                name: "IX_CompanyUsers_UserId",
                table: "CompanyUsers");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "CompanyUsers");

            migrationBuilder.DropColumn(
                name: "RoleId1",
                table: "CompanyUsers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompanyUsers",
                table: "CompanyUsers",
                columns: new[] { "UserId", "CompanyId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CompanyUsers",
                table: "CompanyUsers");

            migrationBuilder.AddColumn<Guid>(
                name: "RoleId",
                table: "CompanyUsers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "RoleId1",
                table: "CompanyUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompanyUsers",
                table: "CompanyUsers",
                columns: new[] { "RoleId", "UserId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyUsers_RoleId1",
                table: "CompanyUsers",
                column: "RoleId1");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyUsers_UserId",
                table: "CompanyUsers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyUsers_AspNetRoles_RoleId1",
                table: "CompanyUsers",
                column: "RoleId1",
                principalTable: "AspNetRoles",
                principalColumn: "Id");
        }
    }
}
