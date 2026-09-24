namespace AllenStock.API.Sales.Domain.Entities;

/// <summary>
/// Entidad raíz que representa un Ticket, Boleta o Factura en el POS.
/// Creado por: Gustavo Alonso Olivares Lao
/// </summary>
public class Sale
{
    public int Id { get; set; }
    
    // Referencias a otros contextos (Caja y Usuarios)[cite: 1]
    public int CashSessionId { get; set; } 
    public int UserId { get; set; } 
    
    public string ReceiptType { get; set; } = "Ticket"; 
    public string ReceiptNumber { get; set; } = string.Empty;
    
    public decimal Subtotal { get; set; }
    public decimal Taxes { get; set; }
    public decimal Total { get; set; }
    
    public string Status { get; set; } = "Completada";
    public DateTime Date { get; set; }

    // Relación: Una venta tiene muchas líneas de detalle
    public ICollection<SaleDetail> Details { get; set; } = new List<SaleDetail>();
}