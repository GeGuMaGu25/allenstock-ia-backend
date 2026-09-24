using System.Text.Json.Serialization;

namespace AllenStock.API.Promotions.Application.DTOs;

public record ApplyPromotionRequestDto(
    [property: JsonPropertyName("producto_id")] int producto_id, 
    [property: JsonPropertyName("porcentaje_descuento")] decimal porcentaje_descuento, 
    [property: JsonPropertyName("justificacion_ia")] string justificacion_ia
);