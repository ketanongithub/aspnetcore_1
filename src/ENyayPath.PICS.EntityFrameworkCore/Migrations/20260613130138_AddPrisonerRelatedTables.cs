using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ENyayPath.PICS.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class AddPrisonerRelatedTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "AK_Prisoner_PrisonerId",
                table: "Prisoner",
                column: "PrisonerId");

            migrationBuilder.CreateTable(
                name: "Document",
                columns: table => new
                {
                    DocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Document", x => x.DocumentId);
                });

            migrationBuilder.CreateTable(
                name: "PrisonerBiometricData",
                columns: table => new
                {
                    PrisonerBiometricDataId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PrisonerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuthenticationType = table.Column<int>(type: "int", nullable: false),
                    BiometricStorageUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrisonerBiometricData", x => x.PrisonerBiometricDataId);
                    table.ForeignKey(
                        name: "FK_PrisonerBiometricData_Lookup_AuthenticationType",
                        column: x => x.AuthenticationType,
                        principalTable: "Lookup",
                        principalColumn: "LookupId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PrisonerBiometricData_Prisoner_PrisonerId",
                        column: x => x.PrisonerId,
                        principalTable: "Prisoner",
                        principalColumn: "PrisonerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PrisonerContactPerson",
                columns: table => new
                {
                    PrisonerContactPersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PrisonerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContactPersonName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Relation = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsTopOnCallList = table.Column<bool>(type: "bit", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrisonerContactPerson", x => x.PrisonerContactPersonId);
                    table.ForeignKey(
                        name: "FK_PrisonerContactPerson_Prisoner_PrisonerId",
                        column: x => x.PrisonerId,
                        principalTable: "Prisoner",
                        principalColumn: "PrisonerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Recharge",
                columns: table => new
                {
                    TransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PrisonerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RechargeType = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recharge", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_Recharge_Lookup_RechargeType",
                        column: x => x.RechargeType,
                        principalTable: "Lookup",
                        principalColumn: "LookupId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Recharge_Prisoner_PrisonerId",
                        column: x => x.PrisonerId,
                        principalTable: "Prisoner",
                        principalColumn: "PrisonerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Wallet",
                columns: table => new
                {
                    WalletId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PrisonerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BalanceAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallet", x => x.WalletId);
                    table.ForeignKey(
                        name: "FK_Wallet_Prisoner_PrisonerId",
                        column: x => x.PrisonerId,
                        principalTable: "Prisoner",
                        principalColumn: "PrisonerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PrisonerContactDetail",
                columns: table => new
                {
                    PrisonerContactDetailsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PrisonerContactPersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsAudioCall = table.Column<bool>(type: "bit", nullable: false),
                    PhoneNumberPrefix = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    SIMAffedavitURL = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsSIMAffedavitUploaded = table.Column<bool>(type: "bit", nullable: true),
                    IsSimValidatedSuccessfully = table.Column<bool>(type: "bit", nullable: true),
                    AppId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    RegisteredName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: true),
                    IsAdharCardUploaded = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrisonerContactDetail", x => x.PrisonerContactDetailsId);
                    table.ForeignKey(
                        name: "FK_PrisonerContactDetail_PrisonerContactPerson_PrisonerContactPersonId",
                        column: x => x.PrisonerContactPersonId,
                        principalTable: "PrisonerContactPerson",
                        principalColumn: "PrisonerContactPersonId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PrisonerContactPersonDocument",
                columns: table => new
                {
                    PrisonerContactPersonDocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PrisonerContactPersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentUploadLink = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsValidDocument = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrisonerContactPersonDocument", x => x.PrisonerContactPersonDocumentId);
                    table.ForeignKey(
                        name: "FK_PrisonerContactPersonDocument_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Document",
                        principalColumn: "DocumentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PrisonerContactPersonDocument_PrisonerContactPerson_PrisonerContactPersonId",
                        column: x => x.PrisonerContactPersonId,
                        principalTable: "PrisonerContactPerson",
                        principalColumn: "PrisonerContactPersonId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PrisonerCallRecord",
                columns: table => new
                {
                    PrisonerCallRecordId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PrisonerContactDetailsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: true),
                    TypeOfCall = table.Column<int>(type: "int", nullable: false),
                    CallCost = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    RecordingPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Remark = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsCallTerminatedByAdmin = table.Column<bool>(type: "bit", nullable: true),
                    IsMonitored = table.Column<bool>(type: "bit", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrisonerCallRecord", x => x.PrisonerCallRecordId);
                    table.ForeignKey(
                        name: "FK_PrisonerCallRecord_Lookup_TypeOfCall",
                        column: x => x.TypeOfCall,
                        principalTable: "Lookup",
                        principalColumn: "LookupId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PrisonerCallRecord_PrisonerContactDetail_PrisonerContactDetailsId",
                        column: x => x.PrisonerContactDetailsId,
                        principalTable: "PrisonerContactDetail",
                        principalColumn: "PrisonerContactDetailsId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PrisonalContactApprovalProcess",
                columns: table => new
                {
                    PrisonalContactApprovalProcessId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PrisonerContactPersonDocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApproverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApproverLevel = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrisonalContactApprovalProcess", x => x.PrisonalContactApprovalProcessId);
                    table.ForeignKey(
                        name: "FK_PrisonalContactApprovalProcess_PrisonerContactPersonDocument_PrisonerContactPersonDocumentId",
                        column: x => x.PrisonerContactPersonDocumentId,
                        principalTable: "PrisonerContactPersonDocument",
                        principalColumn: "PrisonerContactPersonDocumentId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Prisoner_PrisonerId",
                table: "Prisoner",
                column: "PrisonerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PrisonalContactApprovalProcess_PrisonerContactPersonDocumentId",
                table: "PrisonalContactApprovalProcess",
                column: "PrisonerContactPersonDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_PrisonerBiometricData_AuthenticationType",
                table: "PrisonerBiometricData",
                column: "AuthenticationType");

            migrationBuilder.CreateIndex(
                name: "IX_PrisonerBiometricData_PrisonerId",
                table: "PrisonerBiometricData",
                column: "PrisonerId");

            migrationBuilder.CreateIndex(
                name: "IX_PrisonerCallRecord_PrisonerContactDetailsId",
                table: "PrisonerCallRecord",
                column: "PrisonerContactDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_PrisonerCallRecord_TypeOfCall",
                table: "PrisonerCallRecord",
                column: "TypeOfCall");

            migrationBuilder.CreateIndex(
                name: "IX_PrisonerContactDetail_PrisonerContactPersonId",
                table: "PrisonerContactDetail",
                column: "PrisonerContactPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_PrisonerContactPerson_PrisonerId",
                table: "PrisonerContactPerson",
                column: "PrisonerId");

            migrationBuilder.CreateIndex(
                name: "IX_PrisonerContactPersonDocument_DocumentId",
                table: "PrisonerContactPersonDocument",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_PrisonerContactPersonDocument_PrisonerContactPersonId",
                table: "PrisonerContactPersonDocument",
                column: "PrisonerContactPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Recharge_PrisonerId",
                table: "Recharge",
                column: "PrisonerId");

            migrationBuilder.CreateIndex(
                name: "IX_Recharge_RechargeType",
                table: "Recharge",
                column: "RechargeType");

            migrationBuilder.CreateIndex(
                name: "IX_Wallet_PrisonerId",
                table: "Wallet",
                column: "PrisonerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrisonalContactApprovalProcess");

            migrationBuilder.DropTable(
                name: "PrisonerBiometricData");

            migrationBuilder.DropTable(
                name: "PrisonerCallRecord");

            migrationBuilder.DropTable(
                name: "Recharge");

            migrationBuilder.DropTable(
                name: "Wallet");

            migrationBuilder.DropTable(
                name: "PrisonerContactPersonDocument");

            migrationBuilder.DropTable(
                name: "PrisonerContactDetail");

            migrationBuilder.DropTable(
                name: "Document");

            migrationBuilder.DropTable(
                name: "PrisonerContactPerson");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Prisoner_PrisonerId",
                table: "Prisoner");

            migrationBuilder.DropIndex(
                name: "IX_Prisoner_PrisonerId",
                table: "Prisoner");
        }
    }
}
