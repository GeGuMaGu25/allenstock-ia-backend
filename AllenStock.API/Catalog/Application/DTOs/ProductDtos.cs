namespace AllenStock.API.Catalog.Application.DTOs;

public record ProductResponseDto(int id, string nombre, string sku, decimal precio, int stock_actual, string imagen_url, int categoria_id, string categoria_nombre);
public record ProductRequestDto(string nombre, string sku, decimal precio, int stock_actual, string imagen_url, int categoria_id);