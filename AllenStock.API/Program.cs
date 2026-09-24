using AllenStock.API.Catalog.Application.Services;
using AllenStock.API.Catalog.Presentation.Endpoints;
using AllenStock.API.Inventory.Application.Services;
using AllenStock.API.Inventory.Presentation.Endpoints;
using AllenStock.API.Sales.Application.Services;
using AllenStock.API.Sales.Presentation.Endpoints;
using AllenStock.API.Shared.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

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

// ---> 2. INYECCIÓN DE DEPENDENCIAS: Registrar el servicio del catálogo
builder.Services.AddScoped<ICatalogService, CatalogService>();

// Registrar el servicio de inventario
builder.Services.AddScoped<IInventoryService, InventoryService>();

// Registrar el servicio de ventas
builder.Services.AddScoped<ISalesService, SalesService>();

var app = builder.Build();

app.UseCors("VueCorsPolicy");

// 3. Endpoint base de prueba
app.MapGet("/", () => "AllenStock AI API - Funcionando correctamente");

// ---> 3. REGISTRAR LOS ENDPOINTS DE CATÁLOGO
app.MapCatalogEndpoints();

// Registrar los endpoints de inventario
app.MapInventoryEndpoints();

// Registrar los endpoints de ventas
app.MapSalesEndpoints();

app.Run();