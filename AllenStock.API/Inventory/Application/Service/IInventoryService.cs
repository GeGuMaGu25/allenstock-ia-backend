using AllenStock.API.Inventory.Application.DTOs;

namespace AllenStock.API.Inventory.Application.Services;

public interface IInventoryService
{
    Task<bool> RegisterMovementAsync(KardexRequestDto dto);
}