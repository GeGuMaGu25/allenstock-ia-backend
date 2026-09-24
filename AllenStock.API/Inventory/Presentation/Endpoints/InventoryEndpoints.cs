using AllenStock.API.Inventory.Application.DTOs;
using AllenStock.API.Inventory.Application.Services;

namespace AllenStock.API.Inventory.Presentation.Endpoints;

/// <summary>
/// Rutas Minimal API para el Bounded Context de Inventario.
/// Creado por: Gustavo Alonso Olivares Lao
/// </summary>
public static class InventoryEndpoints
{
    public static void MapInventoryEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/inventory");

        group.MapPost("/kardex", async (KardexRequestDto request, IInventoryService inventoryService) =>
        {
            var success = await inventoryService.RegisterMovementAsync(request);
            
            if (success) return Results.Ok(new { message = "Movimiento registrado exitosamente." });
            return Results.BadRequest(new { message = "Error al procesar el inventario." });
        });
    }
}