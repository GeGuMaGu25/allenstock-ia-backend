using AllenStock.API.Catalog.Application.DTOs;
using AllenStock.API.Catalog.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace AllenStock.API.Catalog.Presentation.Endpoints;

public static class CatalogEndpoints
{
    public static void MapCatalogEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/catalog");

        // --- RUTAS CATEGORÍAS ---
        group.MapGet("/categories", async (ICatalogService service) => Results.Ok(await service.GetCategoriesAsync()));
        group.MapPost("/categories", async ([FromBody] CategoryRequestDto dto, ICatalogService service) => Results.Created("", await service.CreateCategoryAsync(dto)));
        group.MapPut("/categories/{id:int}", async (int id, [FromBody] CategoryRequestDto dto, ICatalogService service) => 
        await service.UpdateCategoryAsync(id, dto) ? Results.NoContent() : Results.NotFound());
        group.MapDelete("/categories/{id:int}", async (int id, ICatalogService service) => 
        {
            try { return await service.DeleteCategoryAsync(id) ? Results.NoContent() : Results.NotFound(); }
            catch (Exception ex) { return Results.BadRequest(new { error = ex.Message }); }
        });

        // --- RUTAS PRODUCTOS ---
        group.MapGet("/products", async (ICatalogService service) => Results.Ok(await service.GetProductsAsync()));
        group.MapPost("/products", async ([FromBody] ProductRequestDto dto, ICatalogService service) => Results.Created("", await service.CreateProductAsync(dto)));
        group.MapPut("/products/{id:int}", async (int id, [FromBody] ProductRequestDto dto, ICatalogService service) => 
        await service.UpdateProductAsync(id, dto) ? Results.NoContent() : Results.NotFound());
        group.MapDelete("/products/{id:int}", async (int id, ICatalogService service) => 
            await service.DeleteProductAsync(id) ? Results.NoContent() : Results.NotFound());
    }
}