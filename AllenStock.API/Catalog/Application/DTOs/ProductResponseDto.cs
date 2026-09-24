namespace AllenStock.API.Catalog.Application.DTOs;

/// <summary>
/// DTO para enviar los datos limpios del producto al frontend.
/// Creado por: Gustavo Alonso Olivares Lao
/// </summary>
public record ProductResponseDto(
    int Id, 
    string Codigo_barras, 
    string Nombre, 
    string Categoria, 
    decimal Precio_base
);