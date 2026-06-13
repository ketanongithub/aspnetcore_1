using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ENyayPath.PICS.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class AddPrisonerTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Prisoner",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrisonerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PrisonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PrisonerBatchNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrisonerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Dob = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PrisonerStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AllowedMinutesPerWeek = table.Column<int>(type: "int", nullable: true),
                    IsAudioCallEnabled = table.Column<bool>(type: "bit", nullable: true),
                    IsVideoCallEnabled = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prisoner", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Prisoner");
        }
    }
}
