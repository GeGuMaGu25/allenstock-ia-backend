using AllenStock.API.Sales.Application.DTOs;

namespace AllenStock.API.Sales.Application.Services;

public interface ISalesService
{
    Task<bool> ProcessCheckoutAsync(CheckoutRequestDto dto);
}