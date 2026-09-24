using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AllenStock.API.Migrations
{
    /// <inheritdoc />
    public partial class UpgradeCatalogSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Productos_Categorias_categoria_id",
                table: "Productos");

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

            migrationBuilder.AddColumn<string>(
                name: "imagen_url",
                table: "Productos",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "descripcion",
                table: "Categorias",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "activo",
                table: "Categorias",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_Categorias_categoria_id",
                table: "Productos",
                column: "categoria_id",
                principalTable: "Categorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Productos_Categorias_categoria_id",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "imagen_url",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "activo",
                table: "Categorias");

            migrationBuilder.AlterColumn<string>(
                name: "descripcion",
                table: "Categorias",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_Categorias_categoria_id",
                table: "Productos",
                column: "categoria_id",
                principalTable: "Categorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
