using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ENyayPath.PICS.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class PrisonerDocument : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PrisonerDocuments",
                columns: table => new
                {
                    PrisonerDocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PrisonerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentUploadLink = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsValidDocument = table.Column<bool>(type: "bit", nullable: true),
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
                    table.PrimaryKey("PK_PrisonerDocuments", x => x.PrisonerDocumentId);
                    table.ForeignKey(
                        name: "FK_PrisonerDocuments_Prisoner_PrisonerId",
                        column: x => x.PrisonerId,
                        principalTable: "Prisoner",
                        principalColumn: "PrisonerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PrisonerDocuments_PrisonerId",
                table: "PrisonerDocuments",
                column: "PrisonerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrisonerDocuments");
        }
    }
}
