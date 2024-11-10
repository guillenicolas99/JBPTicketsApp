using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JBPTicketsApp.Migrations
{
    /// <inheritdoc />
    public partial class defaultsValue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Personas_IdPersona",
                table: "Tickets");

            migrationBuilder.AlterColumn<int>(
                name: "IdPersona",
                table: "Tickets",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "NivelesLiderazgo",
                keyColumn: "IdNivelLiderazgo",
                keyValue: 1,
                column: "Nombre",
                value: "Sin Nivel");

            migrationBuilder.UpdateData(
                table: "NivelesLiderazgo",
                keyColumn: "IdNivelLiderazgo",
                keyValue: 2,
                column: "Nombre",
                value: "Miembro");

            migrationBuilder.UpdateData(
                table: "NivelesLiderazgo",
                keyColumn: "IdNivelLiderazgo",
                keyValue: 3,
                column: "Nombre",
                value: "Discipulo");

            migrationBuilder.UpdateData(
                table: "NivelesLiderazgo",
                keyColumn: "IdNivelLiderazgo",
                keyValue: 4,
                column: "Nombre",
                value: "Líder de Casa de Paz");

            migrationBuilder.UpdateData(
                table: "NivelesLiderazgo",
                keyColumn: "IdNivelLiderazgo",
                keyValue: 5,
                column: "Nombre",
                value: "Líder de Discípulo");

            migrationBuilder.UpdateData(
                table: "NivelesLiderazgo",
                keyColumn: "IdNivelLiderazgo",
                keyValue: 6,
                column: "Nombre",
                value: "Diácono");

            migrationBuilder.UpdateData(
                table: "NivelesLiderazgo",
                keyColumn: "IdNivelLiderazgo",
                keyValue: 7,
                column: "Nombre",
                value: "Anciano");

            migrationBuilder.InsertData(
                table: "NivelesLiderazgo",
                columns: new[] { "IdNivelLiderazgo", "Nombre" },
                values: new object[] { 8, "Efesio" });

            migrationBuilder.InsertData(
                table: "Personas",
                columns: new[] { "IdPersona", "IdNivelLiderazgo", "IdRed", "Nombre" },
                values: new object[] { 1, 1, 1, "Sin asignar" });

            migrationBuilder.UpdateData(
                table: "Redes",
                keyColumn: "IdRed",
                keyValue: 1,
                column: "Nombre",
                value: "Sin red");

            migrationBuilder.UpdateData(
                table: "Redes",
                keyColumn: "IdRed",
                keyValue: 2,
                column: "Nombre",
                value: "Adonai");

            migrationBuilder.UpdateData(
                table: "Redes",
                keyColumn: "IdRed",
                keyValue: 3,
                column: "Nombre",
                value: "El Elyon");

            migrationBuilder.UpdateData(
                table: "Redes",
                keyColumn: "IdRed",
                keyValue: 4,
                column: "Nombre",
                value: "El Shaddai");

            migrationBuilder.UpdateData(
                table: "Redes",
                keyColumn: "IdRed",
                keyValue: 5,
                column: "Nombre",
                value: "Emmanuel");

            migrationBuilder.InsertData(
                table: "Redes",
                columns: new[] { "IdRed", "Nombre" },
                values: new object[] { 6, "YAWHE" });

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Personas_IdPersona",
                table: "Tickets",
                column: "IdPersona",
                principalTable: "Personas",
                principalColumn: "IdPersona");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Personas_IdPersona",
                table: "Tickets");

            migrationBuilder.DeleteData(
                table: "NivelesLiderazgo",
                keyColumn: "IdNivelLiderazgo",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Personas",
                keyColumn: "IdPersona",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Redes",
                keyColumn: "IdRed",
                keyValue: 6);

            migrationBuilder.AlterColumn<int>(
                name: "IdPersona",
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "NivelesLiderazgo",
                keyColumn: "IdNivelLiderazgo",
                keyValue: 1,
                column: "Nombre",
                value: "Miembro");

            migrationBuilder.UpdateData(
                table: "NivelesLiderazgo",
                keyColumn: "IdNivelLiderazgo",
                keyValue: 2,
                column: "Nombre",
                value: "Discipulo");

            migrationBuilder.UpdateData(
                table: "NivelesLiderazgo",
                keyColumn: "IdNivelLiderazgo",
                keyValue: 3,
                column: "Nombre",
                value: "Líder de Casa de Paz");

            migrationBuilder.UpdateData(
                table: "NivelesLiderazgo",
                keyColumn: "IdNivelLiderazgo",
                keyValue: 4,
                column: "Nombre",
                value: "Líder de Discípulo");

            migrationBuilder.UpdateData(
                table: "NivelesLiderazgo",
                keyColumn: "IdNivelLiderazgo",
                keyValue: 5,
                column: "Nombre",
                value: "Diácono");

            migrationBuilder.UpdateData(
                table: "NivelesLiderazgo",
                keyColumn: "IdNivelLiderazgo",
                keyValue: 6,
                column: "Nombre",
                value: "Anciano");

            migrationBuilder.UpdateData(
                table: "NivelesLiderazgo",
                keyColumn: "IdNivelLiderazgo",
                keyValue: 7,
                column: "Nombre",
                value: "Efesio");

            migrationBuilder.UpdateData(
                table: "Redes",
                keyColumn: "IdRed",
                keyValue: 1,
                column: "Nombre",
                value: "Adonai");

            migrationBuilder.UpdateData(
                table: "Redes",
                keyColumn: "IdRed",
                keyValue: 2,
                column: "Nombre",
                value: "El Elyon");

            migrationBuilder.UpdateData(
                table: "Redes",
                keyColumn: "IdRed",
                keyValue: 3,
                column: "Nombre",
                value: "El Shaddai");

            migrationBuilder.UpdateData(
                table: "Redes",
                keyColumn: "IdRed",
                keyValue: 4,
                column: "Nombre",
                value: "Emmanuel");

            migrationBuilder.UpdateData(
                table: "Redes",
                keyColumn: "IdRed",
                keyValue: 5,
                column: "Nombre",
                value: "YAWHE");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Personas_IdPersona",
                table: "Tickets",
                column: "IdPersona",
                principalTable: "Personas",
                principalColumn: "IdPersona",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
