namespace AllenStock.API.Purchasing.Application.DTOs;

/// <summary>
/// DTO para enviar el listado de reclamos al dashboard del administrador.
/// Creado por: Gustavo Alonso Olivares Lao
/// </summary>
public record ClaimResponseDto(
    int id, 
    int kardex_id, 
    int proveedor_id, 
    string estado, 
    string? fecha_envio_programado, 
    string observaciones
);