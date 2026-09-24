using AllenStock.API.Sales.Application.DTOs;
using AllenStock.API.Sales.Application.Services;

namespace AllenStock.API.Sales.Presentation.Endpoints;

/// <summary>
/// Rutas Minimal API para el Punto de Venta.
/// Creado por: Gustavo Alonso Olivares Lao
/// </summary>
public static class SalesEndpoints
{
    public static void MapSalesEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/sales");

        group.MapPost("/checkout", async (CheckoutRequestDto request, ISalesService salesService) =>
        {
            var success = await salesService.ProcessCheckoutAsync(request);
            
            if (success) return Results.Ok(new { message = "Venta procesada. Stock actualizado." });
            return Results.BadRequest(new { message = "Transacción rechazada. Verifique el stock." });
        });
    }
}