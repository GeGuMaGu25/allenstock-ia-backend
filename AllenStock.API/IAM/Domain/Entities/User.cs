namespace AllenStock.API.IAM.Domain.Entities;

/// <summary>
/// Entidad que representa a un usuario del sistema (Administrador o Cajero).
/// Creado por: Gustavo Alonso Olivares Lao
/// </summary>
public class User
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    
    // Almacenaremos la contraseña encriptada por seguridad
    public string PasswordHash { get; set; } = string.Empty; 
    
    // Rol determinará a qué pantallas y endpoints tiene acceso
    public string Role { get; set; } = "Cajero"; 
    
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }
}