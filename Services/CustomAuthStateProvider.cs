using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;

namespace Blazor_Respawn_Shop.Services
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly LocalStorageService _localStorage;

        public CustomAuthStateProvider(LocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _localStorage.GetItemAsync("authToken");

            if (string.IsNullOrWhiteSpace(token))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

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
                ["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"]           = "name",
                ["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"]   = "email",
                ["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"]         = "role"
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