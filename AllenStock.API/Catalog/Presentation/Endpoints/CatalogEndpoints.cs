using AllenStock.API.Catalog.Application.Services;

namespace AllenStock.API.Catalog.Presentation.Endpoints;

/// <summary>
/// Rutas Minimal API para el Bounded Context de Catálogo.
/// Creado por: Gustavo Alonso Olivares Lao
/// </summary>
public static class CatalogEndpoints
{
    public static void MapCatalogEndpoints(this IEndpointRouteBuilder app)
    {
        // Agrupamos la URL base
        var group = app.MapGroup("/api/v1/catalog");

        // Definimos el endpoint GET
        group.MapGet("/products", async (ICatalogService catalogService) =>
        {
            var products = await catalogService.GetAllProductsAsync();
            return Results.Ok(products);
        });
    }
}