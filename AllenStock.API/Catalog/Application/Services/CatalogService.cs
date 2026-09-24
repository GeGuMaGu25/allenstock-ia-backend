using AllenStock.API.Catalog.Application.DTOs;
using AllenStock.API.Catalog.Domain.Entities;
using AllenStock.API.Shared.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AllenStock.API.Catalog.Application.Services;

public interface ICatalogService
{
    Task<IEnumerable<CategoryResponseDto>> GetCategoriesAsync();
    Task<CategoryResponseDto> CreateCategoryAsync(CategoryRequestDto dto);
    Task<bool> UpdateCategoryAsync(int id, CategoryRequestDto dto);
    Task<bool> DeleteCategoryAsync(int id);

    Task<IEnumerable<ProductResponseDto>> GetProductsAsync();
    Task<ProductResponseDto> CreateProductAsync(ProductRequestDto dto);
    Task<bool> UpdateProductAsync(int id, ProductRequestDto dto);
    Task<bool> DeleteProductAsync(int id);
}

public class CatalogService : ICatalogService
{
    private readonly AppDbContext _context;

    public CatalogService(AppDbContext context) => _context = context;

    // --- CRUD CATEGORÍAS ---
    public async Task<IEnumerable<CategoryResponseDto>> GetCategoriesAsync()
    {
        return await _context.Categories
            .Select(c => new CategoryResponseDto(c.Id, c.Name, c.Description, c.IsActive))
            .ToListAsync();
    }

    public async Task<CategoryResponseDto> CreateCategoryAsync(CategoryRequestDto dto)
    {
        var category = new Category { Name = dto.nombre, Description = dto.descripcion, IsActive = dto.activo };
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return new CategoryResponseDto(category.Id, category.Name, category.Description, category.IsActive);
    }

    public async Task<bool> UpdateCategoryAsync(int id, CategoryRequestDto dto)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null) return false;
        
        category.Name = dto.nombre;
        category.Description = dto.descripcion;
        category.IsActive = dto.activo;
        
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
        var category = await _context.Categories.Include(c => c.Products).FirstOrDefaultAsync(c => c.Id == id);
        if (category == null) return false;
        if (category.Products.Any()) throw new Exception("No se puede eliminar una categoría que tiene productos asignados.");
        
        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
        return true;
    }

    // --- CRUD PRODUCTOS ---
    public async Task<IEnumerable<ProductResponseDto>> GetProductsAsync()
    {
        return await _context.Products.Include(p => p.Category)
            .Select(p => new ProductResponseDto(p.Id, p.Name, p.Sku, p.Price, p.CurrentStock, p.ImageUrl, p.CategoryId, p.Category.Name))
            .ToListAsync();
    }

    public async Task<ProductResponseDto> CreateProductAsync(ProductRequestDto dto)
    {
        var product = new Product
        {
            Name = dto.nombre, Sku = dto.sku, Price = dto.precio, 
            CurrentStock = dto.stock_actual, ImageUrl = dto.imagen_url, CategoryId = dto.categoria_id
        };
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        
        var category = await _context.Categories.FindAsync(dto.categoria_id);
        return new ProductResponseDto(product.Id, product.Name, product.Sku, product.Price, product.CurrentStock, product.ImageUrl, product.CategoryId, category!.Name);
    }

    public async Task<bool> UpdateProductAsync(int id, ProductRequestDto dto)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return false;
        
        product.Name = dto.nombre; product.Sku = dto.sku; product.Price = dto.precio;
        product.CurrentStock = dto.stock_actual; product.ImageUrl = dto.imagen_url; product.CategoryId = dto.categoria_id;
        
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return false;
        
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return true;
    }
}