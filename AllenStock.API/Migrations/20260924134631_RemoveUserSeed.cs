using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AllenStock.API.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUserSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "correo", "nombre_completo", "activo", "contrasena_hash", "rol" },
                values: new object[] { 1, "admin@allentech.com", "Gustavo Alonso Olivares Lao", true, "$2a$11$0uM.3C/1.BfP/K3nF4L33O3yJz2hR6JgO6.M3XlY1iM3J1.3.3.3.", "Administrador" });
        }
    }
}
