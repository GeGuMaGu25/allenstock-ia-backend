namespace AllenStock.API.Inventory.Domain.Entities;

/// <summary>
/// Entidad de dominio que registra el historial de movimientos de inventario.
/// Creado por: Gustavo Alonso Olivares Lao
/// </summary>
public class Kardex
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    
    // Dejaremos el UserId preparado para cuando conectemos el módulo IAM[cite: 1]
    public int UserId { get; set; } 
    
    public string MovementType { get; set; } = string.Empty; // Entrada, Salida, Ajuste[cite: 1]
    public int Quantity { get; set; }
    public string Reason { get; set; } = string.Empty; // Compra, Venta, Merma, Devolución, Donación[cite: 1]
    public DateTime MovementDate { get; set; }
}