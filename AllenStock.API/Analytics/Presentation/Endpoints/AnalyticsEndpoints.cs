using AllenStock.API.Analytics.Application.Services;

namespace AllenStock.API.Analytics.Presentation.Endpoints;

public static class AnalyticsEndpoints
{
    public static void MapAnalyticsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/analytics");
        group.MapGet("/predictions", async (IAnalyticsService analyticsService) =>
        {
            var predictions = await analyticsService.GetPredictionsAsync();
            return Results.Ok(predictions);
        });
    }
}