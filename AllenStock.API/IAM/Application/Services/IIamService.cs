using AllenStock.API.IAM.Application.DTOs;

namespace AllenStock.API.IAM.Application.Services;

public interface IIamService
{
    Task<SignInResponseDto?> SignInAsync(SignInRequestDto request);
}