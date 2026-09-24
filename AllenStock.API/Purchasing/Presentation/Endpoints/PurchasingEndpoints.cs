using AllenStock.API.Purchasing.Application.Services;

namespace AllenStock.API.Purchasing.Presentation.Endpoints;

/// <summary>
/// Rutas Minimal API para el módulo de Compras y Proveedores.
/// Creado por: Gustavo Alonso Olivares Lao
/// </summary>
public static class PurchasingEndpoints
{
    public static void MapPurchasingEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/purchasing");

        group.MapGet("/claims", async (IPurchasingService purchasingService) =>
        {
            var claims = await purchasingService.GetActiveClaimsAsync();
            return Results.Ok(claims);
        });
    }
}