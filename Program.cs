using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazor_Respawn_Shop;
using Blazor_Respawn_Shop.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Leer la URL base de la API desde appsettings.json
var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"]
    ?? "http://localhost/Ecommerce_Gamer_Api/api/";

// Registrar HttpClient con la URL base configurada
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
});

// Registrar el servicio de productos con inyección de dependencias
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<CartService>();

await builder.Build().RunAsync();