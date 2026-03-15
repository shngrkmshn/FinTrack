using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinTrackPro.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRecurrenceSchedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RecurrenceScheduleId",
                table: "Transactions",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RecurrenceSchedules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TransactionType = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Currency = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RecurrenceType = table.Column<int>(type: "integer", nullable: false),
                    Interval = table.Column<int>(type: "integer", nullable: false),
                    LastGeneratedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecurrenceSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecurrenceSchedules_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecurrenceSchedules_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecurrenceSchedules_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_RecurrenceScheduleId",
                table: "Transactions",
                column: "RecurrenceScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_RecurrenceSchedules_AccountId",
                table: "RecurrenceSchedules",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_RecurrenceSchedules_CategoryId",
                table: "RecurrenceSchedules",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_RecurrenceSchedules_UserId",
                table: "RecurrenceSchedules",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_RecurrenceSchedules_RecurrenceScheduleId",
                table: "Transactions",
                column: "RecurrenceScheduleId",
                principalTable: "RecurrenceSchedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_RecurrenceSchedules_RecurrenceScheduleId",
                table: "Transactions");

            migrationBuilder.DropTable(
                name: "RecurrenceSchedules");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_RecurrenceScheduleId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "RecurrenceScheduleId",
                table: "Transactions");
        }
    }
}
