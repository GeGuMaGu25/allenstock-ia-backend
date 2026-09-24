using AllenStock.API.Cash.Application.DTOs;
using AllenStock.API.Cash.Domain.Entities;
using AllenStock.API.Shared.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AllenStock.API.Cash.Application.Services;

/// <summary>
/// Servicio de aplicación para la gestión del flujo de efectivo.
/// Creado por: Gustavo Alonso Olivares Lao
/// </summary>
public class CashService : ICashService
{
    private readonly AppDbContext _context;

    public CashService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> OpenSessionAsync(OpenCashRequestDto dto)
    {
        var session = new CashSession
        {
            UserId = dto.usuario_id,
            InitialAmount = dto.monto_inicial,
            ExpectedFinalAmount = dto.monto_inicial, // Se actualizará automáticamente al cerrar
            Status = "Abierta",
            OpenedAt = DateTime.UtcNow
        };
        
        _context.CashSessions.Add(session);
        await _context.SaveChangesAsync();
        
        return session.Id; // Devolvemos el ID para que el frontend (Vue) lo guarde y lo pase al módulo de Ventas
    }

    public async Task<bool> CloseSessionAsync(CloseCashRequestDto dto)
    {
        var session = await _context.CashSessions.FindAsync(dto.sesion_id);
        
        if (session == null || session.Status == "Cerrada") 
            return false;

        // Sumarizar todas las ventas exitosas asociadas a esta sesión de caja
        var totalSales = await _context.Sales
            .Where(s => s.CashSessionId == session.Id && s.Status == "Completada")
            .SumAsync(s => s.Total);

        session.ExpectedFinalAmount = session.InitialAmount + totalSales;
        session.RealFinalAmount = dto.monto_final_real;
        session.Status = "Cerrada";
        session.ClosedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }
}