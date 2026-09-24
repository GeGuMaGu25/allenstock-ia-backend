using AllenStock.API.Purchasing.Application.DTOs;

namespace AllenStock.API.Purchasing.Application.Services;

public interface IPurchasingService
{
    Task<IEnumerable<ClaimResponseDto>> GetActiveClaimsAsync();
}