using AllenStock.API.Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AllenStock.API.Shared.Infrastructure.Persistence.Contexts;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // Registramos las tablas
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Mapeo estricto de Categorías
        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Categorias");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasColumnName("nombre").HasMaxLength(100).IsRequired();
            entity.Property(e => e.Description).HasColumnName("descripcion").HasColumnType("text");
        });

        // Mapeo estricto de Productos
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Productos");
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Barcode).HasColumnName("codigo_barras").HasMaxLength(50).IsRequired();
            entity.HasIndex(e => e.Barcode).IsUnique(); // UNIQUE NOT NULL
            
            entity.Property(e => e.Name).HasColumnName("nombre").HasMaxLength(200).IsRequired();
            entity.Property(e => e.PurchasePrice).HasColumnName("precio_compra").HasColumnType("decimal(10,2)").IsRequired();
            entity.Property(e => e.SalePrice).HasColumnName("precio_venta").HasColumnType("decimal(10,2)").IsRequired();
            
            entity.Property(e => e.CurrentStock).HasColumnName("stock_actual").HasDefaultValue(0);
            entity.Property(e => e.MinimumStock).HasColumnName("stock_minimo").HasDefaultValue(5);
            entity.Property(e => e.ExpirationDate).HasColumnName("fecha_vencimiento").HasColumnType("date");
            entity.Property(e => e.Status).HasColumnName("estado").HasMaxLength(20).HasDefaultValue("Activo");
            
            // Foráneas
            entity.Property(e => e.CategoryId).HasColumnName("categoria_id");
            entity.Property(e => e.SupplierId).HasColumnName("proveedor_id");
        });
    }
}