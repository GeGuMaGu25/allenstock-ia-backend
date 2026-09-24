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
        var activePromotions = await _context.Promotions
            .Where(p => p.Status == "Activa")
            .Join(
                _context.Products, 
                promocion => promocion.ProductId, 
                producto => producto.Id, 
                (promocion, producto) => new PromotionResponseDto(
                    promocion.Id,
                    promocion.ProductId,
                    producto.Name,
                    promocion.DiscountPercentage,
                    promocion.Reason,
                    promocion.Status,
                    promocion.CreatedAt.ToString("yyyy-MM-dd HH:mm")
                )
            )
            .OrderByDescending(p => p.fecha_creacion)
            .ToListAsync();

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