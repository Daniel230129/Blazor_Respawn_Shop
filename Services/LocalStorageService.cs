using Microsoft.JSInterop;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging; // <-- Agregado
using Microsoft.Extensions.Configuration; // <-- Agregado

namespace Blazor_Respawn_Shop.Services
{
    public class LocalStorageService
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly ILogger<LocalStorageService> _logger; // <-- Logger
        private readonly IConfiguration _config; // <-- Config

        // Inyectamos todo en el constructor
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
            // La lectura suele ser muy constante, así que evitamos ponerle log para no saturar la consola de F12
            return await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", key);
        }

        // Borra el Token (Para cuando hagamos el Cerrar Sesión)
        public async Task RemoveItemAsync(string key)
        {
            _logger.LogWarning("Eliminando dato de LocalStorage de la llave: {Key}", key);
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
        }
    }
}