using System.Text;
using AllenStock.API.Analytics.Application.Services;
using AllenStock.API.Analytics.Presentation.Endpoints;
using AllenStock.API.Cash.Application.Services;
using AllenStock.API.Cash.Presentation.Endpoints;
using AllenStock.API.Catalog.Application.Services;
using AllenStock.API.Catalog.Presentation.Endpoints;
using AllenStock.API.IAM.Application.Services;
using AllenStock.API.IAM.Presentation.Endpoints;
using AllenStock.API.Inventory.Application.Services;
using AllenStock.API.Inventory.Presentation.Endpoints;
using AllenStock.API.Promotions.Application.Services;
using AllenStock.API.Promotions.Presentation.Endpoints;
using AllenStock.API.Purchasing.Application.Services;
using AllenStock.API.Purchasing.Presentation.Endpoints;
using AllenStock.API.Sales.Application.Services;
using AllenStock.API.Sales.Presentation.Endpoints;
using AllenStock.API.Shared.Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// 1. Obtener la cadena de conexión desde appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 2. Inyectar AppDbContext configurado con el proveedor de PostgreSQL (Npgsql)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// ---> 1. CONFIGURAR CORS PARA QUE VUE (PUERTO 5173) PUEDA CONECTARSE
builder.Services.AddCors(options =>
{
    options.AddPolicy("VueCorsPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });
builder.Services.AddAuthorization(); // Habilitar validación de roles

// ---> 2. INYECCIÓN DEL SERVICIO IAM (Debajo de los otros servicios)
builder.Services.AddScoped<IIamService, IamService>();

// Registrar el servicio de compras
builder.Services.AddScoped<IPurchasingService, PurchasingService>();

// ---> 2. INYECCIÓN DE DEPENDENCIAS: Registrar el servicio del catálogo
builder.Services.AddScoped<ICatalogService, CatalogService>();

// Registrar el servicio de inventario
builder.Services.AddScoped<IInventoryService, InventoryService>();

// Registrar el servicio de ventas
builder.Services.AddScoped<ISalesService, SalesService>();

// Registrar el servicio de caja
builder.Services.AddScoped<ICashService, CashService>();

// Registrar servicios de IA y Promociones
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
builder.Services.AddScoped<IPromotionsService, PromotionsService>();

var app = builder.Build();

app.UseCors("VueCorsPolicy");

// ---> 3. ACTIVAR MIDDLEWARES DE SEGURIDAD (Obligatorio ponerlos antes de los endpoints)
app.UseAuthentication();
app.UseAuthorization();

// 3. Endpoint base de prueba
app.MapGet("/", () => "AllenStock AI API - Funcionando correctamente");

// ---> 3. REGISTRAR LOS ENDPOINTS DE CATÁLOGO
app.MapCatalogEndpoints();

// Registrar los endpoints de inventario
app.MapInventoryEndpoints();

// Registrar los endpoints de ventas
app.MapSalesEndpoints();

// Registrar los endpoints de caja
app.MapCashEndpoints();

// ---> 4. REGISTRAR EL ENDPOINT DE LOGIN
app.MapIamEndpoints();

// Registrar los endpoints de compras
app.MapPurchasingEndpoints();

// Registrar endpoints de IA y Promociones
app.MapAnalyticsEndpoints();
app.MapPromotionsEndpoints();

// Generar usuario administrador al iniciar el servidor
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (!db.Users.Any(u => u.Email == "admin@allentech.com"))
    {
        db.Users.Add(new AllenStock.API.IAM.Domain.Entities.User
        {
            FullName = "Gustavo Alonso Olivares Lao",
            Email = "admin@allentech.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
            Role = "Administrador",
            IsActive = true
        });
        db.SaveChanges();
    }
}

app.Run();