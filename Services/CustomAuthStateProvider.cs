using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging; // <-- Agregado
using Microsoft.Extensions.Configuration; // <-- Agregado

namespace Blazor_Respawn_Shop.Services
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly LocalStorageService _localStorage;
        private readonly ILogger<CustomAuthStateProvider> _logger; // <-- Logger
        private readonly IConfiguration _config; // <-- Config

        // Inyectamos todo en el constructor
        public CustomAuthStateProvider(LocalStorageService localStorage, ILogger<CustomAuthStateProvider> logger, IConfiguration config)
        {
            _localStorage = localStorage;
            _logger = logger;
            _config = config;

            _logger.LogInformation("CustomAuthStateProvider de Blazor inicializado.");
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            _logger.LogInformation("Verificando el estado de autenticación del usuario en el navegador...");
            var token = await _localStorage.GetItemAsync("authToken");

            if (string.IsNullOrWhiteSpace(token))
            {
                _logger.LogWarning("No se encontró token válido. El usuario navega como INVITADO.");
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            _logger.LogInformation("Token encontrado. Construyendo identidad del usuario VIP...");
            var claims = ParseClaimsFromJwt(token);
            var identity = new ClaimsIdentity(claims, "jwt", "name", "role");
            var user = new ClaimsPrincipal(identity);

            return new AuthenticationState(user);
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            // Mapeamos las URIs largas de ClaimTypes de .NET a nombres cortos amigables
            var claimMappings = new Dictionary<string, string>
            {
                ["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"] = "sub",
                ["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"] = "name",
                ["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"] = "email",
                ["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] = "role"
            };

            return keyValuePairs!.Select(kvp =>
            {
                var key = claimMappings.TryGetValue(kvp.Key, out var shortKey) ? shortKey : kvp.Key;
                return new Claim(key, kvp.Value.ToString()!);
            });
        }

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}