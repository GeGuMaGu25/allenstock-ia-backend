namespace AllenStock.API.Purchasing.Domain.Entities;

/// <summary>
/// Entidad que representa un reclamo formal de reposición hacia un proveedor.
/// Creado por: Gustavo Alonso Olivares Lao
/// </summary>
public class SupplierClaim
{
    public int Id { get; set; }
    
    // Referencia al movimiento de Merma en el Kardex
    public int KardexId { get; set; }
    
    public int SupplierId { get; set; }
    
    public string Status { get; set; } = "Notificado"; // Notificado, Aprobado, En_Camino, Resuelto
    
    public DateTime? ScheduledDate { get; set; }
    public string Observations { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}