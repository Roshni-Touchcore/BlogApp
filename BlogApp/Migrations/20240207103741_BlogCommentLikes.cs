using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogApp.Migrations
{
    /// <inheritdoc />
    public partial class BlogCommentLikes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlogCommentLikes",
                columns: table => new
                {
                    BlogCommentLikeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BlogId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CommentBlogCommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogCommentLikes", x => x.BlogCommentLikeId);
                    table.ForeignKey(
                        name: "FK_BlogCommentLikes_BlogComments_CommentBlogCommentId",
                        column: x => x.CommentBlogCommentId,
                        principalTable: "BlogComments",
                        principalColumn: "BlogCommentId");
                    table.ForeignKey(
                        name: "FK_BlogCommentLikes_Blogs_BlogId",
                        column: x => x.BlogId,
                        principalTable: "Blogs",
                        principalColumn: "BlogId");
                    table.ForeignKey(
                        name: "FK_BlogCommentLikes_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlogCommentLikes_BlogId",
                table: "BlogCommentLikes",
                column: "BlogId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogCommentLikes_CommentBlogCommentId",
                table: "BlogCommentLikes",
                column: "CommentBlogCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogCommentLikes_CreatedByUserId",
                table: "BlogCommentLikes",
                column: "CreatedByUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogCommentLikes");
        }
    }
}
