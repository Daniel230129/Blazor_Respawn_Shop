using Microsoft.JSInterop;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace Blazor_Respawn_Shop.Services
{
    public class LocalStorageService
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly ILogger<LocalStorageService> _logger;
        private readonly IConfiguration _config;

        public LocalStorageService(IJSRuntime jsRuntime, ILogger<LocalStorageService> logger, IConfiguration config)
        {
            _jsRuntime = jsRuntime;
            _logger = logger;
            _config = config;

            _logger.LogInformation("LocalStorageService inicializado y enlazado al navegador.");
        }

        // Guarda el Token
        public async Task SetItemAsync(string key, string value)
        {
            _logger.LogInformation("Escribiendo dato en LocalStorage bajo la llave: {Key}", key);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, value);
        }

        // Lee el Token
        public async Task<string?> GetItemAsync(string key)
        {
            return await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", key);
        }

        // Borra el Token
        public async Task RemoveItemAsync(string key)
        {
            _logger.LogWarning("Eliminando dato de LocalStorage de la llave: {Key}", key);
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
        }
    }
}