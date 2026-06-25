using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ENyayPath.PICS.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class AddedCountryIdToPrisoner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CountryId",
                table: "Prisoner",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Prisoner");
        }
    }
}
