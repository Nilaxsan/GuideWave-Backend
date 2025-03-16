using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GuideWave.Migrations
{
    /// <inheritdoc />
    public partial class otpfieldadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Otp",
                table: "Tourists",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Otp",
                table: "Guides",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Otp",
                table: "Tourists");

            migrationBuilder.DropColumn(
                name: "Otp",
                table: "Guides");
        }
    }
}
