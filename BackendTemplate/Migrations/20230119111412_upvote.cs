using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FeedbackHub.Migrations
{
    /// <inheritdoc />
    public partial class upvote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Upvotes",
                table: "Feedbacks");

            migrationBuilder.CreateTable(
                name: "FeedbackUpvotes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FeedbackId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UpvoteDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedbackUpvotes", x => new { x.Id, x.FeedbackId, x.UserId });
                    table.ForeignKey(
                        name: "FK_FeedbackUpvotes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeedbackUpvotes_Feedbacks_FeedbackId",
                        column: x => x.FeedbackId,
                        principalTable: "Feedbacks",
                        principalColumn: "FeedbackId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeedbackUpvotes_FeedbackId",
                table: "FeedbackUpvotes",
                column: "FeedbackId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedbackUpvotes_UserId",
                table: "FeedbackUpvotes",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeedbackUpvotes");

            migrationBuilder.AddColumn<int>(
                name: "Upvotes",
                table: "Feedbacks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
