using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JBPTicketsApp.Migrations
{
    /// <inheritdoc />
    public partial class FirstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    IdCategoria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.IdCategoria);
                });

            migrationBuilder.CreateTable(
                name: "Eventos",
                columns: table => new
                {
                    IdEvento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Eventos", x => x.IdEvento);
                });

            migrationBuilder.CreateTable(
                name: "NivelesLiderazgo",
                columns: table => new
                {
                    IdNivelLiderazgo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NivelesLiderazgo", x => x.IdNivelLiderazgo);
                });

            migrationBuilder.CreateTable(
                name: "Redes",
                columns: table => new
                {
                    IdRed = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Redes", x => x.IdRed);
                });

            migrationBuilder.CreateTable(
                name: "Personas",
                columns: table => new
                {
                    IdPersona = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdRed = table.Column<int>(type: "int", nullable: false),
                    IdNivelLiderazgo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personas", x => x.IdPersona);
                    table.ForeignKey(
                        name: "FK_Personas_NivelesLiderazgo_IdNivelLiderazgo",
                        column: x => x.IdNivelLiderazgo,
                        principalTable: "NivelesLiderazgo",
                        principalColumn: "IdNivelLiderazgo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Personas_Redes_IdRed",
                        column: x => x.IdRed,
                        principalTable: "Redes",
                        principalColumn: "IdRed",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    IdTicket = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Abono = table.Column<double>(type: "float", nullable: true),
                    Precio = table.Column<double>(type: "float", nullable: false),
                    Descuento = table.Column<double>(type: "float", nullable: true),
                    FechaDescuento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdEvento = table.Column<int>(type: "int", nullable: false),
                    IdCategoria = table.Column<int>(type: "int", nullable: false),
                    IdPersona = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.IdTicket);
                    table.ForeignKey(
                        name: "FK_Tickets_Categorias_IdCategoria",
                        column: x => x.IdCategoria,
                        principalTable: "Categorias",
                        principalColumn: "IdCategoria",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tickets_Eventos_IdEvento",
                        column: x => x.IdEvento,
                        principalTable: "Eventos",
                        principalColumn: "IdEvento",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tickets_Personas_IdPersona",
                        column: x => x.IdPersona,
                        principalTable: "Personas",
                        principalColumn: "IdPersona",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categorias",
                columns: new[] { "IdCategoria", "Nombre" },
                values: new object[,]
                {
                    { 1, "Premium" },
                    { 2, "VIP" },
                    { 3, "General" }
                });

            migrationBuilder.InsertData(
                table: "NivelesLiderazgo",
                columns: new[] { "IdNivelLiderazgo", "Nombre" },
                values: new object[,]
                {
                    { 1, "Miembro" },
                    { 2, "Discipulo" },
                    { 3, "Líder de Casa de Paz" },
                    { 4, "Líder de Discípulo" },
                    { 5, "Diácono" },
                    { 6, "Anciano" },
                    { 7, "Efesio" }
                });

            migrationBuilder.InsertData(
                table: "Redes",
                columns: new[] { "IdRed", "Nombre" },
                values: new object[,]
                {
                    { 1, "Adonai" },
                    { 2, "El Elyon" },
                    { 3, "El Shaddai" },
                    { 4, "Emmanuel" },
                    { 5, "YAWHE" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Personas_IdNivelLiderazgo",
                table: "Personas",
                column: "IdNivelLiderazgo");

            migrationBuilder.CreateIndex(
                name: "IX_Personas_IdRed",
                table: "Personas",
                column: "IdRed");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_IdCategoria",
                table: "Tickets",
                column: "IdCategoria");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_IdEvento",
                table: "Tickets",
                column: "IdEvento");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_IdPersona",
                table: "Tickets",
                column: "IdPersona");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "Eventos");

            migrationBuilder.DropTable(
                name: "Personas");

            migrationBuilder.DropTable(
                name: "NivelesLiderazgo");

            migrationBuilder.DropTable(
                name: "Redes");
        }
    }
}
