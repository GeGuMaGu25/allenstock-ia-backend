namespace AllenStock.API.Catalog.Application.DTOs;

public record CategoryResponseDto(int id, string nombre, string descripcion, bool activo);
public record CategoryRequestDto(string nombre, string descripcion, bool activo);