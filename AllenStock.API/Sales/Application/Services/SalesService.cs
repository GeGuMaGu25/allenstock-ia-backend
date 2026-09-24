using AllenStock.API.Sales.Application.DTOs;
using AllenStock.API.Sales.Domain.Entities;
using AllenStock.API.Shared.Infrastructure.Persistence.Contexts;

namespace AllenStock.API.Sales.Application.Services;

/// <summary>
/// Servicio de aplicación para procesar ventas y descontar inventario en bloque.
/// Creado por: Gustavo Alonso Olivares Lao
/// </summary>
public class SalesService : ISalesService
{
    private readonly AppDbContext _context;

    public SalesService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ProcessCheckoutAsync(CheckoutRequestDto dto)
    {
        // Iniciamos una transacción SQL para proteger la integridad de los datos
        using var transaction = await _context.Database.BeginTransactionAsync();
        
        try
        {
            // 1. Calcular totales
            decimal subtotal = dto.items.Sum(i => i.cantidad * i.precio_unitario);
            decimal taxes = subtotal * 0.18m; // IGV del 18%
            decimal total = subtotal + taxes;

            // 2. Crear la Venta (Ticket)
            var sale = new Sale
            {
                CashSessionId = dto.sesion_caja_id,
                UserId = dto.usuario_id,
                ReceiptNumber = $"TKT-{DateTime.UtcNow.Ticks.ToString()[^6..]}", // Número aleatorio basado en el tiempo
                Subtotal = subtotal,
                Taxes = taxes,
                Total = total
            };
            
            _context.Sales.Add(sale);
            await _context.SaveChangesAsync(); // Guardamos para que PostgreSQL genere el ID de la venta

            // 3. Procesar detalles y descontar stock
            foreach (var item in dto.items)
            {
                var detail = new SaleDetail
                {
                    SaleId = sale.Id,
                    ProductId = item.producto_id,
                    Quantity = item.cantidad,
                    UnitPrice = item.precio_unitario,
                    Subtotal = item.cantidad * item.precio_unitario
                };
                _context.SaleDetails.Add(detail);

                // Descontar del catálogo principal
                var product = await _context.Products.FindAsync(item.producto_id);
                if (product != null)
                {
                    if (product.CurrentStock < item.cantidad)
                        throw new Exception($"Stock insuficiente para el producto ID {product.Id}");
                        
                    product.CurrentStock -= item.cantidad;
                }
            }

            // 4. Confirmar todo
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return true;
        }
        catch (Exception)
        {
            // Si falta stock o falla la conexión, se cancela la boleta y el descuento de stock
            await transaction.RollbackAsync();
            return false;
        }
    }
}