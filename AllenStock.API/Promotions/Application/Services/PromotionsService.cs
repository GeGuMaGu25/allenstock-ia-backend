using AllenStock.API.Promotions.Application.DTOs;
using AllenStock.API.Promotions.Domain.Entities;
using AllenStock.API.Shared.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AllenStock.API.Promotions.Application.Services;

public interface IPromotionsService
{
    Task<bool> ApplyDiscountAsync(ApplyPromotionRequestDto dto);
    Task<IEnumerable<PromotionResponseDto>> GetActivePromotionsAsync(); // <- Nueva línea
}

/// <summary>
/// Servicio de aplicación para registrar campañas de descuentos aprobadas.
/// Creado por: Gustavo Alonso Olivares Lao
/// </summary>
public class PromotionsService : IPromotionsService
{
    private readonly AppDbContext _context;

    public PromotionsService(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<PromotionResponseDto>> GetActivePromotionsAsync()
    {
        // 1. Fase SQL: Traemos los datos crudos a la memoria (se traduce perfectamente a PostgreSQL)
        var rawData = await _context.Promotions
            .Where(p => p.Status == "Activa")
            .Join(
                _context.Products, 
                promocion => promocion.ProductId, 
                producto => producto.Id, 
                (promocion, producto) => new { promocion, producto } // Objeto anónimo temporal
            )
            .OrderByDescending(x => x.promocion.CreatedAt) // Ordenamos por el DateTime real
            .ToListAsync(); // Traemos de la BD a la RAM

        // 2. Fase Memoria: Transformamos los datos al DTO y formateamos la fecha con C#
        var activePromotions = rawData.Select(x => new PromotionResponseDto(
            x.promocion.Id,
            x.promocion.ProductId,
            x.producto.Name,
            x.promocion.DiscountPercentage,
            x.promocion.Reason,
            x.promocion.Status,
            x.promocion.CreatedAt.ToString("yyyy-MM-dd HH:mm") // Ahora sí funciona porque ya no es SQL
        ));

        return activePromotions;
    }

    public async Task<bool> ApplyDiscountAsync(ApplyPromotionRequestDto dto)
    {
        var promotion = new Promotion
        {
            ProductId = dto.producto_id,
            DiscountPercentage = dto.porcentaje_descuento,
            Reason = dto.justificacion_ia,
            Status = "Activa"
        };

        _context.Promotions.Add(promotion);
        await _context.SaveChangesAsync();
        return true;
    }
}