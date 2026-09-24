namespace AllenStock.API.Promotions.Domain.Entities;

/// <summary>
/// Entidad que representa un descuento aplicado a un producto, generalmente sugerido por IA.
/// Creado por: Gustavo Alonso Olivares Lao
/// </summary>
public class Promotion
{
    public int Id { get; set; }
    
    // Referencia al producto del catálogo que recibirá el descuento
    public int ProductId { get; set; }
    
    public decimal DiscountPercentage { get; set; }
    public string Status { get; set; } = "Activa";
    
    // Guardaremos la justificación de la IA para tener un historial del por qué se tomó la decisión
    public string Reason { get; set; } = string.Empty; 
    
    public DateTime CreatedAt { get; set; }
}