using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AllenStock.API.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialCatalog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categorias",
                columns: new[] { "Id", "descripcion", "nombre" },
                values: new object[,]
                {
                    { 1, "Laptops, monitores y componentes", "Electrónica" },
                    { 2, "Periféricos y cables", "Accesorios" }
                });

            migrationBuilder.InsertData(
                table: "Productos",
                columns: new[] { "Id", "codigo_barras", "categoria_id", "stock_actual", "fecha_vencimiento", "stock_minimo", "nombre", "precio_compra", "precio_venta", "estado", "proveedor_id" },
                values: new object[,]
                {
                    { 1, "7751234567890", 1, 10, null, 5, "Laptop ASUS ROG", 1200.00m, 1500.00m, "Activo", 1 },
                    { 2, "7750987654321", 2, 25, null, 5, "Teclado Mecánico", 50.00m, 85.50m, "Activo", 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Productos",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Productos",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
