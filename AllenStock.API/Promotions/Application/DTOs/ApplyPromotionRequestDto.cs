namespace AllenStock.API.Promotions.Application.DTOs;

/// <summary>
/// DTO para recibir la aprobación del administrador y guardar el descuento.
/// Creado por: Gustavo Alonso Olivares Lao
/// </summary>
public record ApplyPromotionRequestDto(
    int producto_id, 
    decimal porcentaje_descuento, 
    string justificacion_ia
);