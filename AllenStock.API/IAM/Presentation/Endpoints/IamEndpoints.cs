using AllenStock.API.IAM.Application.DTOs;
using AllenStock.API.IAM.Application.Services;

namespace AllenStock.API.IAM.Presentation.Endpoints;

/// <summary>
/// Rutas Minimal API para la identidad y accesos.
/// Creado por: Gustavo Alonso Olivares Lao
/// </summary>
public static class IamEndpoints
{
    public static void MapIamEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/iam/auth");

        group.MapPost("/sign-in", async (SignInRequestDto request, IIamService iamService) =>
        {
            var result = await iamService.SignInAsync(request);
            
            if (result != null) return Results.Ok(result);
            return Results.Unauthorized();
        });
    }
}