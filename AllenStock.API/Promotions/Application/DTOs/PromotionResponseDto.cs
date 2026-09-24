namespace AllenStock.API.Promotions.Application.DTOs;

/// <summary>
/// DTO para enviar el listado de promociones activas al frontend.
/// Creado por: Gustavo Alonso Olivares Lao
/// </summary>
public record PromotionResponseDto(
    int id,
    int producto_id,
    string nombre_producto,
    decimal porcentaje_descuento,
    string justificacion_ia,
    string estado,
    string fecha_creacion
);