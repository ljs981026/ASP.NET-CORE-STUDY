using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetCore.Services.Migrations
{
    public partial class AddingUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_UserRolesByUser_RoleId_UserId",
                table: "UserRolesByUser");

            migrationBuilder.CreateIndex(
                name: "IX_UserRolesByUser_RoleId",
                table: "UserRolesByUser",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserRolesByUser_RoleId",
                table: "UserRolesByUser");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_UserRolesByUser_RoleId_UserId",
                table: "UserRolesByUser",
                columns: new[] { "RoleId", "UserId" });
        }
    }
}
