using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AllenStock.API.IAM.Application.DTOs;
using AllenStock.API.Shared.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace AllenStock.API.IAM.Application.Services;

/// <summary>
/// Servicio de aplicación para validar credenciales y generar tokens JWT.
/// Creado por: Gustavo Alonso Olivares Lao
/// </summary>
public class IamService : IIamService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public IamService(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<SignInResponseDto?> SignInAsync(SignInRequestDto request)
    {
        // 1. Buscar usuario por correo
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.correo && u.IsActive);
        if (user == null) return null;

        // 2. Verificar contraseña con BCrypt
        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.contrasena, user.PasswordHash);
        if (!isPasswordValid) return null;

        // 3. Generar Token JWT
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            }),
            Expires = DateTime.UtcNow.AddHours(8), // El token dura 1 turno laboral
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtString = tokenHandler.WriteToken(token);

        // 4. Retornar los datos al frontend
        return new SignInResponseDto(user.Id, user.FullName, user.Role, jwtString);
    }
}