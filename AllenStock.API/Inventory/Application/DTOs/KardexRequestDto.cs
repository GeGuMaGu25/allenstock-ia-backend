namespace AllenStock.API.Inventory.Application.DTOs;

/// <summary>
/// DTO para recibir la petición de movimiento de inventario desde el frontend.
/// Creado por: Gustavo Alonso Olivares Lao
/// </summary>
public record KardexRequestDto(
    int producto_id, 
    string tipo_movimiento, 
    int cantidad, 
    string motivo
);