namespace AllenStock.API.Sales.Domain.Entities;

/// <summary>
/// Entidad que representa una línea de producto dentro de un ticket de venta.
/// Creado por: Gustavo Alonso Olivares Lao
/// </summary>
public class SaleDetail
{
    public int Id { get; set; }
    
    // Clave foránea hacia la Venta padre
    public int SaleId { get; set; }
    public Sale Sale { get; set; } = null!;
    
    // Referencia al producto vendido[cite: 1]
    public int ProductId { get; set; }
    
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Discount { get; set; } = 0.00m;
    public decimal Subtotal { get; set; }
}