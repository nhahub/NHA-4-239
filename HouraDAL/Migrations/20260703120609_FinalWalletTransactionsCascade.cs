using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HouraDAL.Migrations
{
    /// <inheritdoc />
    public partial class FinalWalletTransactionsCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "FiatPackages",
                columns: table => new
                {
                    PackageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PackageName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    HoursCount = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FiatPackages", x => x.PackageId);
                });

            migrationBuilder.CreateTable(
                name: "ROLES",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ROLES", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "USERS",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USERS", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "SERVICE_POSTINGS",
                columns: table => new
                {
                    PostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    PostType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EstimatedDurationInMinutes = table.Column<int>(type: "int", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SERVICE_POSTINGS", x => x.PostId);
                    table.ForeignKey(
                        name: "FK_SERVICE_POSTINGS_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SERVICE_POSTINGS_USERS_UserId",
                        column: x => x.UserId,
                        principalTable: "USERS",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TIME_WALLETS",
                columns: table => new
                {
                    WalletId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BalanceInMinutes = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TIME_WALLETS", x => x.WalletId);
                    table.ForeignKey(
                        name: "FK_TIME_WALLETS_USERS_UserId",
                        column: x => x.UserId,
                        principalTable: "USERS",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TimePurchaseInvoices",
                columns: table => new
                {
                    InvoiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PackageId = table.Column<int>(type: "int", nullable: false),
                    AmountPaid = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HoursCredited = table.Column<int>(type: "int", nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimePurchaseInvoices", x => x.InvoiceId);
                    table.ForeignKey(
                        name: "FK_TimePurchaseInvoices_FiatPackages_PackageId",
                        column: x => x.PackageId,
                        principalTable: "FiatPackages",
                        principalColumn: "PackageId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TimePurchaseInvoices_USERS_UserId",
                        column: x => x.UserId,
                        principalTable: "USERS",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "USER_ROLES",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USER_ROLES", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_USER_ROLES_ROLES_RoleId",
                        column: x => x.RoleId,
                        principalTable: "ROLES",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_USER_ROLES_USERS_UserId",
                        column: x => x.UserId,
                        principalTable: "USERS",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "POST_APPLICATIONS",
                columns: table => new
                {
                    ApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplicantUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Pending"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_POST_APPLICATIONS", x => x.ApplicationId);
                    table.ForeignKey(
                        name: "FK_POST_APPLICATIONS_SERVICE_POSTINGS_PostId",
                        column: x => x.PostId,
                        principalTable: "SERVICE_POSTINGS",
                        principalColumn: "PostId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_POST_APPLICATIONS_USERS_ApplicantUserId",
                        column: x => x.ApplicantUserId,
                        principalTable: "USERS",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SERVICE_TRANSACTIONS",
                columns: table => new
                {
                    TransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProviderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReceiverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DurationInMinutes = table.Column<int>(type: "int", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Pending"),
                    ResolvedByAdminId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ResolutionNotes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SERVICE_TRANSACTIONS", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_SERVICE_TRANSACTIONS_POST_APPLICATIONS_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "POST_APPLICATIONS",
                        principalColumn: "ApplicationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SERVICE_TRANSACTIONS_SERVICE_POSTINGS_PostId",
                        column: x => x.PostId,
                        principalTable: "SERVICE_POSTINGS",
                        principalColumn: "PostId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SERVICE_TRANSACTIONS_USERS_ProviderId",
                        column: x => x.ProviderId,
                        principalTable: "USERS",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SERVICE_TRANSACTIONS_USERS_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "USERS",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "REVIEWS",
                columns: table => new
                {
                    ReviewId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceTransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReviewerUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RevieweeUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REVIEWS", x => x.ReviewId);
                    table.ForeignKey(
                        name: "FK_REVIEWS_SERVICE_TRANSACTIONS_ServiceTransactionId",
                        column: x => x.ServiceTransactionId,
                        principalTable: "SERVICE_TRANSACTIONS",
                        principalColumn: "TransactionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_REVIEWS_USERS_RevieweeUserId",
                        column: x => x.RevieweeUserId,
                        principalTable: "USERS",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_REVIEWS_USERS_ReviewerUserId",
                        column: x => x.ReviewerUserId,
                        principalTable: "USERS",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WALLET_TRANSACTIONS",
                columns: table => new
                {
                    WalletTransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WalletId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AmountInMinutes = table.Column<int>(type: "int", nullable: false),
                    BalanceAfter = table.Column<int>(type: "int", nullable: false),
                    SourceType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ServiceTransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    InvoiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WALLET_TRANSACTIONS", x => x.WalletTransactionId);
                    table.ForeignKey(
                        name: "FK_WALLET_TRANSACTIONS_SERVICE_TRANSACTIONS_ServiceTransactionId",
                        column: x => x.ServiceTransactionId,
                        principalTable: "SERVICE_TRANSACTIONS",
                        principalColumn: "TransactionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WALLET_TRANSACTIONS_TIME_WALLETS_WalletId",
                        column: x => x.WalletId,
                        principalTable: "TIME_WALLETS",
                        principalColumn: "WalletId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WALLET_TRANSACTIONS_TimePurchaseInvoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "TimePurchaseInvoices",
                        principalColumn: "InvoiceId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_POST_APPLICATIONS_ApplicantUserId",
                table: "POST_APPLICATIONS",
                column: "ApplicantUserId");

            migrationBuilder.CreateIndex(
                name: "IX_POST_APPLICATIONS_PostId",
                table: "POST_APPLICATIONS",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_REVIEWS_RevieweeUserId",
                table: "REVIEWS",
                column: "RevieweeUserId");

            migrationBuilder.CreateIndex(
                name: "IX_REVIEWS_ReviewerUserId",
                table: "REVIEWS",
                column: "ReviewerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_REVIEWS_ServiceTransactionId",
                table: "REVIEWS",
                column: "ServiceTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_SERVICE_POSTINGS_CategoryId",
                table: "SERVICE_POSTINGS",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SERVICE_POSTINGS_UserId",
                table: "SERVICE_POSTINGS",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SERVICE_TRANSACTIONS_ApplicationId",
                table: "SERVICE_TRANSACTIONS",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_SERVICE_TRANSACTIONS_PostId",
                table: "SERVICE_TRANSACTIONS",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_SERVICE_TRANSACTIONS_ProviderId",
                table: "SERVICE_TRANSACTIONS",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_SERVICE_TRANSACTIONS_ReceiverId",
                table: "SERVICE_TRANSACTIONS",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_TIME_WALLETS_UserId",
                table: "TIME_WALLETS",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TimePurchaseInvoices_PackageId",
                table: "TimePurchaseInvoices",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_TimePurchaseInvoices_UserId",
                table: "TimePurchaseInvoices",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_USER_ROLES_RoleId",
                table: "USER_ROLES",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_USERS_Email",
                table: "USERS",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WALLET_TRANSACTIONS_InvoiceId",
                table: "WALLET_TRANSACTIONS",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_WALLET_TRANSACTIONS_ServiceTransactionId",
                table: "WALLET_TRANSACTIONS",
                column: "ServiceTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_WALLET_TRANSACTIONS_WalletId",
                table: "WALLET_TRANSACTIONS",
                column: "WalletId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "REVIEWS");

            migrationBuilder.DropTable(
                name: "USER_ROLES");

            migrationBuilder.DropTable(
                name: "WALLET_TRANSACTIONS");

            migrationBuilder.DropTable(
                name: "ROLES");

            migrationBuilder.DropTable(
                name: "SERVICE_TRANSACTIONS");

            migrationBuilder.DropTable(
                name: "TIME_WALLETS");

            migrationBuilder.DropTable(
                name: "TimePurchaseInvoices");

            migrationBuilder.DropTable(
                name: "POST_APPLICATIONS");

            migrationBuilder.DropTable(
                name: "FiatPackages");

            migrationBuilder.DropTable(
                name: "SERVICE_POSTINGS");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "USERS");
        }
    }
}
