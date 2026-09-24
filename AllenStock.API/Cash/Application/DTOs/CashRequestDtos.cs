namespace AllenStock.API.Cash.Application.DTOs;

/// <summary>
/// DTOs para la apertura y cierre de caja.
/// Creado por: Gustavo Alonso Olivares Lao
/// </summary>
public record OpenCashRequestDto(int usuario_id, decimal monto_inicial);

public record CloseCashRequestDto(int sesion_id, decimal monto_final_real);