using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AvondaleIslamicCentre.Migrations
{
    /// <inheritdoc />
    public partial class noticescleanup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Notices");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Notices",
                type: "datetime2",
                nullable: true);
        }
    }
}
