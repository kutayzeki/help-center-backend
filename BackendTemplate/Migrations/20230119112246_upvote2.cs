using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelpCenter.Migrations
{
    /// <inheritdoc />
    public partial class upvote2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FeedbackUpvotes",
                table: "FeedbackUpvotes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FeedbackUpvotes",
                table: "FeedbackUpvotes",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FeedbackUpvotes",
                table: "FeedbackUpvotes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FeedbackUpvotes",
                table: "FeedbackUpvotes",
                columns: new[] { "Id", "FeedbackId", "UserId" });
        }
    }
}
