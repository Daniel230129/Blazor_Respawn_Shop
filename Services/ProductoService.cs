using Blazor_Respawn_Shop.Models;
using System.Net.Http.Json;

namespace Blazor_Respawn_Shop.Services
{
    /// <summary>
    /// Servicio concreto que consume la API REST del backend Ecommerce_Gamer_Api.
    /// Recibe HttpClient, ILogger e IConfiguration por inyección de dependencias.
    /// </summary>
    public class ProductoService : IProductoService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ProductoService> _logger;
        private readonly IConfiguration _configuration;

        // URL base leída desde appsettings.json → ApiSettings:BaseUrl
        private readonly string _baseUrl;

        public ProductoService(
            HttpClient httpClient,
            ILogger<ProductoService> logger,
            IConfiguration configuration)
        {
            _httpClient = httpClient;
            _logger = logger;
            _configuration = configuration;

            // Leer la URL base desde la configuración
            _baseUrl = _configuration["ApiSettings:BaseUrl"]
                ?? throw new InvalidOperationException("ApiSettings:BaseUrl no está configurada en appsettings.json");

            _logger.LogInformation("ProductoService inicializado con BaseUrl: {BaseUrl}", _baseUrl);
        }

        /// <summary>
        /// Obtiene todos los productos desde GET /api/Productos.
        /// </summary>
        public async Task<List<ProductoDto>> GetProductosAsync()
        {
            try
            {
                _logger.LogInformation("Solicitando catálogo completo de productos a la API.");

                var productos = await _httpClient.GetFromJsonAsync<List<ProductoDto>>($"{_baseUrl}Productos");

                _logger.LogInformation("Se obtuvieron {Count} productos del backend.", productos?.Count ?? 0);

                return productos ?? new List<ProductoDto>();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error de red al obtener productos del backend. URL: {Url}", $"{_baseUrl}Productos");
                return new List<ProductoDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener productos.");
                return new List<ProductoDto>();
            }
        }

        /// <summary>
        /// Obtiene productos filtrados por categoría.
        /// El backend no tiene endpoint de filtrado, así que filtramos localmente.
        /// </summary>
        public async Task<List<ProductoDto>> GetProductosPorCategoriaAsync(string categoria)
        {
            try
            {
                _logger.LogInformation("Filtrando productos por categoría: {Categoria}", categoria);

                // Obtenemos todos y filtramos en memoria
                var todos = await GetProductosAsync();

                if (string.IsNullOrWhiteSpace(categoria) || categoria == "Todos")
                    return todos;

                var filtrados = todos
                    .Where(p => p.Categoria?.Nombre?.Equals(categoria, StringComparison.OrdinalIgnoreCase) == true)
                    .ToList();

                _logger.LogInformation("Filtrado completado: {Count} productos en categoría '{Categoria}'.", filtrados.Count, categoria);

                return filtrados;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al filtrar productos por categoría '{Categoria}'.", categoria);
                return new List<ProductoDto>();
            }
        }

        /// <summary>
        /// Obtiene un producto por ID desde GET /api/Productos/{id}.
        /// </summary>
        public async Task<ProductoDto?> GetProductoByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Solicitando producto con ID {Id} a la API.", id);

                var producto = await _httpClient.GetFromJsonAsync<ProductoDto>($"{_baseUrl}Productos/{id}");

                if (producto == null)
                    _logger.LogWarning("La API devolvió null para el producto con ID {Id}.", id);

                return producto;
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _logger.LogWarning("Producto con ID {Id} no encontrado en el backend (404).", id);
                return null;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error de red al obtener el producto con ID {Id}.", id);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener el producto con ID {Id}.", id);
                return null;
            }
        }

        /// <summary>
        /// Llama al endpoint REAL de IA en el backend para autocompletar los datos del juego.
        /// </summary>
        public async Task<RespuestaIADto?> AutocompletarProductoConIAAsync(string nombreJuego)
        {
            try
            {
                // Usamos el RespuestaIADto para que atrape todas las variables del JSON sin perder nada
                return await _httpClient.GetFromJsonAsync<RespuestaIADto>($"{_baseUrl}Ia/autocompletar/{nombreJuego}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al conectar con la IA.");
                return null;
            }
        }
        public async Task<bool> CrearProductoAsync(ProductoDto nuevoProducto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}Productos", nuevoProducto);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar el producto en la base de datos.");
                return false;
            }
        }
    }
}
