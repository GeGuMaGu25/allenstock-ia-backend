using AllenStock.API.Analytics.Application.DTOs;
using AllenStock.API.Shared.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AllenStock.API.Analytics.Application.Services;

public interface IAnalyticsService
{
    Task<IEnumerable<AiRecommendationResponseDto>> GetPredictionsAsync();
}

public class AnalyticsService : IAnalyticsService
{
    private readonly AppDbContext _context;

    // Inyectamos la conexión a PostgreSQL
    public AnalyticsService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AiRecommendationResponseDto>> GetPredictionsAsync()
    {
        await Task.Delay(1500); // Simulamos el tiempo de respuesta de la API de OpenAI

        // 1. Verificamos si el descuento sugerido ya fue aprobado y guardado en la BD
        bool promotionAlreadyExists = await _context.Promotions
            .AnyAsync(p => p.ProductId == 2 && p.Status == "Activa");

        // 2. Si ya existe, devolvemos una lista vacía (no hay nuevas sugerencias)
        if (promotionAlreadyExists)
        {
            return new List<AiRecommendationResponseDto>();
        }

        // 3. Si no existe, enviamos la sugerencia al frontend
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