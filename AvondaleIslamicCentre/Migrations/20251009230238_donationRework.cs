using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AvondaleIslamicCentre.Migrations
{
    /// <inheritdoc />
    public partial class donationRework : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DonorName",
                table: "Donations");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DonorName",
                table: "Donations",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
