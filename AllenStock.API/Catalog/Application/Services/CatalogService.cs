using AllenStock.API.Catalog.Application.DTOs;
using AllenStock.API.Shared.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AllenStock.API.Catalog.Application.Services;

/// <summary>
/// Servicio de aplicación para la lógica del Catálogo.
/// Creado por: Gustavo Alonso Olivares Lao
/// </summary>
public class CatalogService : ICatalogService
{
    private readonly AppDbContext _context;

    // Inyección de Dependencias: Pedimos el contexto de la base de datos
    public CatalogService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync()
    {
        return await _context.Products
            .Include(p => p.Category) // Hacemos un JOIN con la tabla Categorías
            .Select(p => new ProductResponseDto(
                p.Id,
                p.Barcode,
                p.Name,
                p.Category.Name, // Sacamos el nombre de la categoría relacionada
                p.SalePrice
            ))
            .ToListAsync();
    }
}