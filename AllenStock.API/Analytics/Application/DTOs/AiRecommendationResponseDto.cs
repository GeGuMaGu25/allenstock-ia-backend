namespace AllenStock.API.Analytics.Application.DTOs;

/// <summary>
/// DTO para enviar la predicción de la IA al panel de control.
/// Creado por: Gustavo Alonso Olivares Lao
/// </summary>
public record AiRecommendationResponseDto(
    int id, 
    int producto_id, 
    string accion_sugerida, 
    double probabilidad_exito, 
    string justificacion_ia
);