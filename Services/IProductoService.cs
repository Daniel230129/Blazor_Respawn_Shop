using Blazor_Respawn_Shop.Models;

namespace Blazor_Respawn_Shop.Services
{
    /// <summary>
    /// Interfaz del servicio de productos. Toda comunicación con el backend va por aquí.
    /// </summary>
    public interface IProductoService
    {
        /// <summary>Obtiene todos los productos del catálogo.</summary>
        Task<List<ProductoDto>> GetProductosAsync();

        /// <summary>Obtiene productos filtrados por nombre de categoría.</summary>
        Task<List<ProductoDto>> GetProductosPorCategoriaAsync(string categoria);

        /// <summary>Obtiene un producto específico por su ID.</summary>
        Task<ProductoDto?> GetProductoByIdAsync(int id);
        Task<RespuestaIADto?> AutocompletarProductoConIAAsync(string nombreJuego);
        Task<bool> CrearProductoAsync(ProductoDto nuevoProducto);
        Task<bool> EliminarAsync(int id);
        Task<bool> ActualizarProductoAsync(int id, ProductoDto productoModificado);
    }
}