using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogApp.Migrations
{
    /// <inheritdoc />
    public partial class RelationShip1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogComments_BlogComments_BlogCommentId1",
                table: "BlogComments");

            migrationBuilder.DropIndex(
                name: "IX_BlogComments_BlogCommentId1",
                table: "BlogComments");

            migrationBuilder.DropColumn(
                name: "BlogCommentId1",
                table: "BlogComments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BlogCommentId1",
                table: "BlogComments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BlogComments_BlogCommentId1",
                table: "BlogComments",
                column: "BlogCommentId1");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogComments_BlogComments_BlogCommentId1",
                table: "BlogComments",
                column: "BlogCommentId1",
                principalTable: "BlogComments",
                principalColumn: "BlogCommentId");
        }
    }
}
