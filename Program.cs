using Blazor_Respawn_Shop;
using Blazor_Respawn_Shop.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

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
builder.Services.AddScoped<LocalStorageService>();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, Blazor_Respawn_Shop.Services.CustomAuthStateProvider>();
builder.Services.AddScoped<Blazor_Respawn_Shop.Services.PedidoService>();

await builder.Build().RunAsync();