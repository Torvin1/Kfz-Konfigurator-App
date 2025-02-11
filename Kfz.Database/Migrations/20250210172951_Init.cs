using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Kfz.Database.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Fuels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EcoFriendlinessRating = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fuels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Manufacturers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BasePrice = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manufacturers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Options",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Options", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Motors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    FuelId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Motors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Motors_Fuels_FuelId",
                        column: x => x.FuelId,
                        principalTable: "Fuels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ManufacturerId = table.Column<int>(type: "int", nullable: false),
                    MotorId = table.Column<int>(type: "int", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FuelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Fuels_FuelId",
                        column: x => x.FuelId,
                        principalTable: "Fuels",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Orders_Manufacturers_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalTable: "Manufacturers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Motors_MotorId",
                        column: x => x.MotorId,
                        principalTable: "Motors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderOptions",
                columns: table => new
                {
                    OptionsId = table.Column<int>(type: "int", nullable: false),
                    OrdersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderOptions", x => new { x.OptionsId, x.OrdersId });
                    table.ForeignKey(
                        name: "FK_OrderOptions_Options_OptionsId",
                        column: x => x.OptionsId,
                        principalTable: "Options",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderOptions_Orders_OrdersId",
                        column: x => x.OrdersId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Fuels",
                columns: new[] { "Id", "DisplayName", "EcoFriendlinessRating" },
                values: new object[,]
                {
                    { 1, "Elektro", 1 },
                    { 2, "Hybrid", 2 },
                    { 3, "Benzin", 3 },
                    { 4, "Diesel", 4 }
                });

            migrationBuilder.InsertData(
                table: "Manufacturers",
                columns: new[] { "Id", "BasePrice", "DisplayName" },
                values: new object[,]
                {
                    { 1, 15000, "Volkswagen" },
                    { 2, 20000, "Opel" },
                    { 3, 30000, "BMW" }
                });

            migrationBuilder.InsertData(
                table: "Options",
                columns: new[] { "Id", "DisplayName", "Price" },
                values: new object[,]
                {
                    { 1, "Klimaanlage", 2000 },
                    { 2, "Alufelgen", 1000 },
                    { 3, "Navigation", 500 },
                    { 4, "Subwoofer", 100 }
                });

            migrationBuilder.InsertData(
                table: "Motors",
                columns: new[] { "Id", "DisplayName", "FuelId", "Price" },
                values: new object[,]
                {
                    { 1, "Benzin 1.6", 3, 1500 },
                    { 2, "Benzin 2.5", 3, 5000 },
                    { 3, "Diesel 1.8", 4, 2000 },
                    { 4, "Diesel 3.5", 4, 4000 },
                    { 5, "Hybrid 1.0", 2, 2500 },
                    { 6, "Hybrid 2.2", 2, 3500 },
                    { 7, "Elektro 1.0", 1, 4500 },
                    { 8, "Elektro 1.5", 1, 6000 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Motors_FuelId",
                table: "Motors",
                column: "FuelId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderOptions_OrdersId",
                table: "OrderOptions",
                column: "OrdersId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_FuelId",
                table: "Orders",
                column: "FuelId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ManufacturerId",
                table: "Orders",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_MotorId",
                table: "Orders",
                column: "MotorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderOptions");

            migrationBuilder.DropTable(
                name: "Options");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Manufacturers");

            migrationBuilder.DropTable(
                name: "Motors");

            migrationBuilder.DropTable(
                name: "Fuels");
        }
    }
}
