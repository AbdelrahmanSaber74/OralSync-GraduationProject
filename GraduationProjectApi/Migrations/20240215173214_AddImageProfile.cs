using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityManagerServerApi.Migrations
{
    /// <inheritdoc />
    public partial class AddImageProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfileImage",
                table: "Students",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfileImage",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfileImage",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileImage",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "ProfileImage",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "ProfileImage",
                table: "Doctors");
        }
    }
}
