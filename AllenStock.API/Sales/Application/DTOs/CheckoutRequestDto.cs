namespace AllenStock.API.Sales.Application.DTOs;

/// <summary>
/// DTOs para recibir el carrito de compras y procesar el pago.
/// Creado por: Gustavo Alonso Olivares Lao
/// </summary>
public record CartItemDto(int producto_id, int cantidad, decimal precio_unitario);

public record CheckoutRequestDto(int sesion_caja_id, int usuario_id, List<CartItemDto> items);