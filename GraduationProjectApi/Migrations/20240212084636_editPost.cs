using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityManagerServerApi.Migrations
{
    /// <inheritdoc />
    public partial class editPost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Posts",
                newName: "TimeUpdated");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Posts",
                newName: "TimeCreated");

            migrationBuilder.AddColumn<string>(
                name: "DateCreated",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DateUpdated",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "DateUpdated",
                table: "Posts");

            migrationBuilder.RenameColumn(
                name: "TimeUpdated",
                table: "Posts",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "TimeCreated",
                table: "Posts",
                newName: "CreatedAt");
        }
    }
}
