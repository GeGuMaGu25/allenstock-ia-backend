namespace AllenStock.API.Catalog.Domain.Entities;

/// <summary>
/// Entidad maestra de productos.
/// Creado por: Gustavo Alonso Olivares Lao
/// </summary>
public class Product
{
    public int Id { get; set; }
    
    // Cambiado de Barcode a Sku para mantener compatibilidad con DTOs y CatalogService
    public string Sku { get; set; } = string.Empty; 
    
    public string Name { get; set; } = string.Empty;
    public decimal PurchasePrice { get; set; }
    
    // Cambiado de SalePrice a Price para mantener compatibilidad con DTOs y CatalogService
    public decimal Price { get; set; } 
    
    public int CurrentStock { get; set; }
    public int MinimumStock { get; set; } = 5;
    public DateTime? ExpirationDate { get; set; }
    public string Status { get; set; } = "Activo";

    // Soporte para imágenes
    public string ImageUrl { get; set; } = string.Empty;

    // Claves Foráneas
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    // Lo dejaremos como entero hasta que creemos el módulo Purchasing
    public int SupplierId { get; set; } 
}