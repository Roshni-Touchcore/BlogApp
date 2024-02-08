using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogApp.Migrations
{
    /// <inheritdoc />
    public partial class UserConnection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserConnections",
                columns: table => new
                {
                    UserConnectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FollowRequesteddByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FollowAcceptedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsAccepted = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserConnections", x => x.UserConnectionId);
                    table.ForeignKey(
                        name: "FK_UserConnections_Users_FollowAcceptedByUserId",
                        column: x => x.FollowAcceptedByUserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_UserConnections_Users_FollowRequesteddByUserId",
                        column: x => x.FollowRequesteddByUserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserConnections_FollowAcceptedByUserId",
                table: "UserConnections",
                column: "FollowAcceptedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserConnections_FollowRequesteddByUserId",
                table: "UserConnections",
                column: "FollowRequesteddByUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserConnections");
        }
    }
}
