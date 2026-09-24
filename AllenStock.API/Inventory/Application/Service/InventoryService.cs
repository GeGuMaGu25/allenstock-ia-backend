using AllenStock.API.Inventory.Application.DTOs;
using AllenStock.API.Inventory.Domain.Entities;
using AllenStock.API.Shared.Infrastructure.Persistence.Contexts;

namespace AllenStock.API.Inventory.Application.Services;

/// <summary>
/// Servicio de aplicación para gestionar entradas, salidas y mermas.
/// Creado por: Gustavo Alonso Olivares Lao
/// </summary>
public class InventoryService : IInventoryService
{
    private readonly AppDbContext _context;

    public InventoryService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> RegisterMovementAsync(KardexRequestDto dto)
    {
        // 1. Guardar el registro en el Kardex
        var kardex = new Kardex
        {
            ProductId = dto.producto_id,
            UserId = 1, // Temporal: Hasta que conectemos el token JWT del módulo IAM
            MovementType = dto.tipo_movimiento,
            Quantity = dto.cantidad,
            Reason = dto.motivo,
            MovementDate = DateTime.UtcNow
        };
        _context.KardexRecords.Add(kardex);

        // 2. Modificar el stock real del Catálogo (único dueño de la verdad)
        var product = await _context.Products.FindAsync(dto.producto_id);
        if (product != null)
        {
            if (dto.tipo_movimiento == "Salida")
            {
                product.CurrentStock -= dto.cantidad;
            }
            else if (dto.tipo_movimiento == "Entrada")
            {
                product.CurrentStock += dto.cantidad;
            }
            
            // Nota arquitectónica: Aquí emitiremos el evento 'MermaRegistrada' 
            // para que el módulo de Compras lo escuche en el futuro.
        }

        // 3. Confirmar la transacción en PostgreSQL
        await _context.SaveChangesAsync();
        return true;
    }
}