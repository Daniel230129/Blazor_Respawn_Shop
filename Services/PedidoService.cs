using System.Net.Http.Json;
using System.Net.Http.Headers;
using Blazor_Respawn_Shop.Models;

namespace Blazor_Respawn_Shop.Services
{
    public class PedidoService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly LocalStorageService _localStorage;
        private readonly string _baseUrl;

        public PedidoService(HttpClient httpClient, IConfiguration configuration, LocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _localStorage = localStorage;
            _baseUrl = _configuration["ApiSettings:BaseUrl"] ?? throw new InvalidOperationException("Falta BaseUrl");
        }

        public async Task<List<PedidoDto>> GetMisPedidosAsync()
        {
            try
            {
                // 1. Buscamos el pase VIP en la memoria del navegador
                var token = await _localStorage.GetItemAsync("authToken");

                // 2. Si lo tenemos, se lo pegamos a la petición
                if (!string.IsNullOrWhiteSpace(token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                // 3. Hacemos la llamada a la API
                var pedidos = await _httpClient.GetFromJsonAsync<List<PedidoDto>>($"{_baseUrl}Pedidos");
                return pedidos ?? new List<PedidoDto>();
            }
            catch (Exception)
            {
                // Si hay error o no hay pedidos, devolvemos lista vacía
                return new List<PedidoDto>();
            }
        }

        public async Task<bool> ProcesarPedidoAsync(object datosDelPedido)
        {
            try
            {
                // 1. Buscamos el pase VIP
                var token = await _localStorage.GetItemAsync("authToken");
                if (!string.IsNullOrWhiteSpace(token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                // 2. Disparamos la orden al endpoint de tu compañero
                var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}Pedidos/procesar", datosDelPedido);

                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Actualiza el estado de un pedido en la base de datos.
        /// </summary>
        public async Task<bool> ActualizarEstadoAsync(int pedidoId, string nuevoEstado)
        {
            try
            {
                var payload = new { Estado = nuevoEstado };
                var response = await _httpClient.PutAsJsonAsync($"Pedidos/{pedidoId}/estado", payload);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar estado: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Obtiene un pedido específico por su ID.
        /// </summary>
        public async Task<Blazor_Respawn_Shop.Models.PedidoDto?> GetPedidoByIdAsync(int id)
        {
            try
            {
                // Como la API quizás no tenga un GET por ID, descargamos la lista y filtramos
                var pedidos = await GetMisPedidosAsync();
                return pedidos.FirstOrDefault(p => p.Id == id);
            }
            catch
            {
                return null;
            }
        }
    }
}