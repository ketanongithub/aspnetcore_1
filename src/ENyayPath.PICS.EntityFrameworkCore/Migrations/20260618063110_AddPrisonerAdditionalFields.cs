using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ENyayPath.PICS.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class AddPrisonerAdditionalFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "Prisoner",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MotherName",
                table: "Prisoner",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SonDaughterOf",
                table: "Prisoner",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SpouseName",
                table: "Prisoner",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "StateId",
                table: "Prisoner",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Prisoner");

            migrationBuilder.DropColumn(
                name: "MotherName",
                table: "Prisoner");

            migrationBuilder.DropColumn(
                name: "SonDaughterOf",
                table: "Prisoner");

            migrationBuilder.DropColumn(
                name: "SpouseName",
                table: "Prisoner");

            migrationBuilder.DropColumn(
                name: "StateId",
                table: "Prisoner");
        }
    }
}
