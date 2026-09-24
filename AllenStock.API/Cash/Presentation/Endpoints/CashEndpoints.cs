using AllenStock.API.Cash.Application.DTOs;
using AllenStock.API.Cash.Application.Services;

namespace AllenStock.API.Cash.Presentation.Endpoints;

/// <summary>
/// Rutas Minimal API para la gestión de caja.
/// Creado por: Gustavo Alonso Olivares Lao
/// </summary>
public static class CashEndpoints
{
    public static void MapCashEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/cash");

        group.MapPost("/open", async (OpenCashRequestDto request, ICashService cashService) =>
        {
            var sessionId = await cashService.OpenSessionAsync(request);
            return Results.Ok(new { message = "Caja abierta correctamente.", sessionId });
        });

        group.MapPost("/close", async (CloseCashRequestDto request, ICashService cashService) =>
        {
            var success = await cashService.CloseSessionAsync(request);
            if (success) return Results.Ok(new { message = "Arqueo de caja finalizado exitosamente." });
            
            return Results.BadRequest(new { message = "Error al procesar el cierre. Verifique que la sesión exista y esté abierta." });
        });
    }
}