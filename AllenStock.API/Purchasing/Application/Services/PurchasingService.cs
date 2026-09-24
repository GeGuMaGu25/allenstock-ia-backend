using AllenStock.API.Purchasing.Application.DTOs;
using AllenStock.API.Shared.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AllenStock.API.Purchasing.Application.Services;

/// <summary>
/// Servicio de aplicación para consultar el estado de reposiciones y reclamos.
/// Creado por: Gustavo Alonso Olivares Lao
/// </summary>
public class PurchasingService : IPurchasingService
{
    private readonly AppDbContext _context;

    public PurchasingService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ClaimResponseDto>> GetActiveClaimsAsync()
    {
        return await _context.SupplierClaims
            .OrderByDescending(c => c.CreatedAt)
            .Select(c => new ClaimResponseDto(
                c.Id,
                c.KardexId,
                c.SupplierId,
                c.Status,
                c.ScheduledDate.HasValue ? c.ScheduledDate.Value.ToString("yyyy-MM-dd") : null,
                c.Observations
            ))
            .ToListAsync();
    }
}