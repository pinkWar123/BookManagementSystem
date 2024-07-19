using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class SecondCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "customers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    customerName = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    totalDept = table.Column<int>(type: "int", nullable: false),
                    address = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    phoneNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "inventoryReports",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    reportMonth = table.Column<int>(type: "int", nullable: false),
                    reportYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inventoryReports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentReceives",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    amount = table.Column<int>(type: "int", nullable: false),
                    customerID = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentReceives", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentReceive_Customer",
                        column: x => x.customerID,
                        principalTable: "customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InventoryReportDetails",
                columns: table => new
                {
                    ReportID = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    BookID = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    initalStock = table.Column<int>(type: "int", nullable: false),
                    finalStock = table.Column<int>(type: "int", nullable: false),
                    additionalStock = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryReportDetails", x => new { x.ReportID, x.BookID });
                    table.ForeignKey(
                        name: "FK_InventoryReportDetail_Book",
                        column: x => x.BookID,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InventoryReportDetail_InventoryReport",
                        column: x => x.ReportID,
                        principalTable: "inventoryReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryReportDetails_BookID",
                table: "InventoryReportDetails",
                column: "BookID");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentReceives_customerID",
                table: "PaymentReceives",
                column: "customerID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InventoryReportDetails");

            migrationBuilder.DropTable(
                name: "PaymentReceives");

            migrationBuilder.DropTable(
                name: "inventoryReports");

            migrationBuilder.DropTable(
                name: "customers");
        }
    }
}
