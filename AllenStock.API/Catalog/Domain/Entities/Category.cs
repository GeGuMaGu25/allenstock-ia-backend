namespace AllenStock.API.Catalog.Domain.Entities;

/// <summary>
/// Entidad que representa una familia o categoría de productos.
/// Creado por: Gustavo Alonso Olivares Lao
/// </summary>
public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    
    // Relación: Una categoría tiene muchos productos
    public ICollection<Product> Products { get; set; } = new List<Product>();
}