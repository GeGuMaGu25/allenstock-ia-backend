using AllenStock.API.Cash.Domain.Entities;
using AllenStock.API.Catalog.Domain.Entities;
using AllenStock.API.Inventory.Domain.Entities;
using AllenStock.API.Sales.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace AllenStock.API.Shared.Infrastructure.Persistence.Contexts;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // Registramos las tablas
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    
    public DbSet<Kardex> KardexRecords { get; set; }
    
    public DbSet<Sale> Sales { get; set; }
    public DbSet<SaleDetail> SaleDetails { get; set; }
    
    public DbSet<CashSession> CashSessions { get; set; }

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
        
        // Mapeo estricto del Kardex
        modelBuilder.Entity<Kardex>(entity =>
        {
            entity.ToTable("Kardex");
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.ProductId).HasColumnName("producto_id").IsRequired();
            entity.Property(e => e.UserId).HasColumnName("usuario_id");
            
            entity.Property(e => e.MovementType).HasColumnName("tipo_movimiento").HasMaxLength(20).IsRequired();
            entity.Property(e => e.Quantity).HasColumnName("cantidad").IsRequired();
            entity.Property(e => e.Reason).HasColumnName("motivo").HasMaxLength(50).IsRequired();
            
            // Usamos la función nativa de PostgreSQL para la fecha por defecto
            entity.Property(e => e.MovementDate)
                .HasColumnName("fecha_movimiento")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        });
        
        // Mapeo estricto de Ventas
        modelBuilder.Entity<Sale>(entity =>
        {
            entity.ToTable("Ventas");
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.CashSessionId).HasColumnName("sesion_caja_id");
            entity.Property(e => e.UserId).HasColumnName("usuario_id");
            
            entity.Property(e => e.ReceiptType).HasColumnName("comprobante_tipo").HasMaxLength(20).IsRequired();
            entity.Property(e => e.ReceiptNumber).HasColumnName("comprobante_numero").HasMaxLength(50).IsRequired();
            
            entity.Property(e => e.Subtotal).HasColumnName("subtotal").HasColumnType("decimal(10,2)").IsRequired();
            entity.Property(e => e.Taxes).HasColumnName("impuestos").HasColumnType("decimal(10,2)").IsRequired();
            entity.Property(e => e.Total).HasColumnName("total").HasColumnType("decimal(10,2)").IsRequired();
            
            entity.Property(e => e.Status).HasColumnName("estado").HasMaxLength(20).HasDefaultValue("Completada");
            entity.Property(e => e.Date).HasColumnName("fecha").HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        // Mapeo estricto de Detalles de Venta
        modelBuilder.Entity<SaleDetail>(entity =>
        {
            entity.ToTable("Detalle_Ventas");
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.SaleId).HasColumnName("venta_id").IsRequired();
            entity.Property(e => e.ProductId).HasColumnName("producto_id").IsRequired();
            
            entity.Property(e => e.Quantity).HasColumnName("cantidad").IsRequired();
            entity.Property(e => e.UnitPrice).HasColumnName("precio_unitario").HasColumnType("decimal(10,2)").IsRequired();
            entity.Property(e => e.Discount).HasColumnName("descuento").HasColumnType("decimal(10,2)").HasDefaultValue(0.00m);
            entity.Property(e => e.Subtotal).HasColumnName("subtotal").HasColumnType("decimal(10,2)").IsRequired();
            
            // Configurar relación 1 a muchos (Una venta tiene muchos detalles)
            entity.HasOne(d => d.Sale)
                  .WithMany(s => s.Details)
                  .HasForeignKey(d => d.SaleId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
        
        // Mapeo estricto de Sesiones de Caja
        modelBuilder.Entity<CashSession>(entity =>
        {
            entity.ToTable("Sesiones_Caja");
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.UserId).HasColumnName("usuario_id").IsRequired();
            
            entity.Property(e => e.InitialAmount).HasColumnName("monto_inicial").HasColumnType("decimal(10,2)").IsRequired();
            entity.Property(e => e.ExpectedFinalAmount).HasColumnName("monto_final_esperado").HasColumnType("decimal(10,2)").IsRequired();
            entity.Property(e => e.RealFinalAmount).HasColumnName("monto_final_real").HasColumnType("decimal(10,2)");
            
            entity.Property(e => e.Status).HasColumnName("estado").HasMaxLength(20).HasDefaultValue("Abierta");
            
            entity.Property(e => e.OpenedAt).HasColumnName("fecha_apertura").HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.ClosedAt).HasColumnName("fecha_cierre");
        });
        
        // ---> SEEDING: Inserción de datos iniciales
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Electrónica", Description = "Laptops, monitores y componentes" },
            new Category { Id = 2, Name = "Accesorios", Description = "Periféricos y cables" }
        );

        modelBuilder.Entity<Product>().HasData(
            new Product { 
                Id = 1, 
                Barcode = "7751234567890", 
                Name = "Laptop ASUS ROG", 
                CategoryId = 1, 
                PurchasePrice = 1200.00m, 
                SalePrice = 1500.00m, 
                CurrentStock = 10, 
                SupplierId = 1 
            },
            new Product { 
                Id = 2, 
                Barcode = "7750987654321", 
                Name = "Teclado Mecánico", 
                CategoryId = 2, 
                PurchasePrice = 50.00m, 
                SalePrice = 85.50m, 
                CurrentStock = 25, 
                SupplierId = 1 
            }
        );
    }
}