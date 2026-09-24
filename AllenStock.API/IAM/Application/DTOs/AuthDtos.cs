namespace AllenStock.API.IAM.Application.DTOs;

/// <summary>
/// DTOs para el inicio de sesión.
/// Creado por: Gustavo Alonso Olivares Lao
/// </summary>
public record SignInRequestDto(string correo, string contrasena);

public record SignInResponseDto(int id, string nombre_completo, string rol, string token);