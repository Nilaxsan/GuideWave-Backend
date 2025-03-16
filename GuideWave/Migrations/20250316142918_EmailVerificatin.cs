using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GuideWave.Migrations
{
    /// <inheritdoc />
    public partial class EmailVerificatin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmailVerificationToken",
                table: "Tourists",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsEmailVerified",
                table: "Tourists",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Availability",
                table: "Places",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailVerificationToken",
                table: "Guides",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsEmailVerified",
                table: "Guides",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailVerificationToken",
                table: "Tourists");

            migrationBuilder.DropColumn(
                name: "IsEmailVerified",
                table: "Tourists");

            migrationBuilder.DropColumn(
                name: "Availability",
                table: "Places");

            migrationBuilder.DropColumn(
                name: "EmailVerificationToken",
                table: "Guides");

            migrationBuilder.DropColumn(
                name: "IsEmailVerified",
                table: "Guides");
        }
    }
}
