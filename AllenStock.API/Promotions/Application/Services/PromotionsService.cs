using AllenStock.API.Promotions.Application.DTOs;
using AllenStock.API.Promotions.Domain.Entities;
using AllenStock.API.Shared.Infrastructure.Persistence.Contexts;

namespace AllenStock.API.Promotions.Application.Services;

public interface IPromotionsService
{
    Task<bool> ApplyDiscountAsync(ApplyPromotionRequestDto dto);
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

    public async Task<bool> ApplyDiscountAsync(ApplyPromotionRequestDto dto)
    {
        var promotion = new Promotion
        {
            ProductId = dto.producto_id,
            DiscountPercentage = dto.porcentaje_descuento,
            Reason = dto.justificacion_ia,
            Status = "Activa",
            CreatedAt = DateTime.UtcNow
        };

        _context.Promotions.Add(promotion);
        await _context.SaveChangesAsync();
        return true;
    }
}