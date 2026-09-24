namespace AllenStock.API.Cash.Domain.Entities;

/// <summary>
/// Entidad que representa la apertura, operaciones y cierre de una caja registradora.
/// Creado por: Gustavo Alonso Olivares Lao
/// </summary>
public class CashSession
{
    public int Id { get; set; }
    
    // Referencia al usuario (Cajero) que abrió la caja
    public int UserId { get; set; }
    
    public decimal InitialAmount { get; set; }
    public decimal ExpectedFinalAmount { get; set; }
    public decimal? RealFinalAmount { get; set; }
    
    // Estado puede ser 'Abierta' o 'Cerrada'
    public string Status { get; set; } = "Abierta";
    
    public DateTime OpenedAt { get; set; }
    public DateTime? ClosedAt { get; set; }
}