using AllenStock.API.Analytics.Application.DTOs;

namespace AllenStock.API.Analytics.Application.Services;

public interface IAnalyticsService
{
    Task<IEnumerable<AiRecommendationResponseDto>> GetPredictionsAsync();
}

/// <summary>
/// Servicio de aplicación que interactuará con los modelos predictivos.
/// Creado por: Gustavo Alonso Olivares Lao
/// </summary>
public class AnalyticsService : IAnalyticsService
{
    public async Task<IEnumerable<AiRecommendationResponseDto>> GetPredictionsAsync()
    {
        // En un entorno de producción, aquí inyectarías la SDK de OpenAI 
        // para analizar la tabla de Ventas y el Kardex. 
        // Por ahora, devolveremos una estructura idéntica a la que generaría el LLM.
        await Task.Delay(1500); // Simulamos el tiempo de respuesta de la API de IA

        return new List<AiRecommendationResponseDto>
        {
            new AiRecommendationResponseDto(
                id: 1,
                producto_id: 2, // Teclado Mecánico
                accion_sugerida: "Promocion",
                probabilidad_exito: 0.88,
                justificacion_ia: "El análisis histórico indica baja rotación en este trimestre. Se sugiere aplicar un 20% de descuento para liberar stock retenido."
            )
        };
    }
}