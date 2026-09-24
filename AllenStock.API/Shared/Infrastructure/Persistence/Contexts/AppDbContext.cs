using Microsoft.EntityFrameworkCore;

namespace AllenStock.API.Shared.Infrastructure.Persistence.Contexts;

/// <summary>
/// Contexto central de base de datos para AllenStock AI.
/// Creado por: Gustavo Alonso Olivares Lao
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // Aquí registraremos los DbSets (Tablas) de cada Bounded Context (Catalog, Inventory, etc.)
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Aquí aplicaremos las reglas estrictas de las tablas usando Fluent API (Nombres, Claves foráneas, etc.)
    }
}