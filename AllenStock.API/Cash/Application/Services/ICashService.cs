using AllenStock.API.Cash.Application.DTOs;

namespace AllenStock.API.Cash.Application.Services;

public interface ICashService
{
    Task<int> OpenSessionAsync(OpenCashRequestDto dto);
    Task<bool> CloseSessionAsync(CloseCashRequestDto dto);
}