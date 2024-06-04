using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProductOrders.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CustomerName = table.Column<string>(type: "text", nullable: false),
                    CustomerEmail = table.Column<string>(type: "text", nullable: true),
                    ShippingAddress = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "Amount", "CreatedAt", "CustomerEmail", "CustomerName", "ShippingAddress", "Status" },
                values: new object[,]
                {
                    { new Guid("67749942-13d7-4826-97bc-21fbb7d394c7"), 200m, new DateTime(2024, 5, 30, 19, 52, 19, 300, DateTimeKind.Utc).AddTicks(931), "", "Jane Doe", "456 Main St, New York, NY 10030", 0 },
                    { new Guid("e3bd7748-4b1f-447e-aa68-d1bc4bff2460"), 100m, new DateTime(2024, 5, 30, 19, 52, 19, 300, DateTimeKind.Utc).AddTicks(919), "", "John Doe", "123 Main St, New York, NY 10030", 0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
