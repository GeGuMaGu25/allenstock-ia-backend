using AllenStock.API.Promotions.Application.DTOs;
using AllenStock.API.Promotions.Application.Services;

namespace AllenStock.API.Promotions.Presentation.Endpoints;

public static class PromotionsEndpoints
{
    public static void MapPromotionsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/promotions");
        group.MapPost("/apply", async (ApplyPromotionRequestDto request, IPromotionsService promotionsService) =>
        {
            var success = await promotionsService.ApplyDiscountAsync(request);
            if (success) return Results.Ok(new { message = "Promoción registrada exitosamente en la base de datos." });
            return Results.BadRequest();
        });
        
        // NUEVO: Endpoint para listar promociones
        group.MapGet("/active", async (IPromotionsService promotionsService) =>
        {
            var promotions = await promotionsService.GetActivePromotionsAsync();
            return Results.Ok(promotions);
        });
    }
}