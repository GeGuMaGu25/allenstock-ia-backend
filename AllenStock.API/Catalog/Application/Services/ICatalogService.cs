using AllenStock.API.Catalog.Application.DTOs;

namespace AllenStock.API.Catalog.Application.Services;

public interface ICatalogService
{
    Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync();
}