// Modelos DTO del frontend para deserializar el JSON del backend

namespace Blazor_Respawn_Shop.Models
{
    /// <summary>
    /// DTO que representa un producto del catálogo, adaptado al modelo real del backend.
    /// </summary>
    public class ProductoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public bool EsDigital { get; set; }

        // Relación con categoría (objeto anidado del backend)
        public int CategoriaId { get; set; }
        public CategoriaDto? Categoria { get; set; }

        public string? Genero { get; set; }
        public string? RequisitosSistema { get; set; }
        public string? TrailerUrl { get; set; }

        public int DescuentoAplicado { get; set; } = 0;

        // Lista de imágenes del producto
        public List<ImagenProductoDto> Imagenes { get; set; } = new();

        // Lista de reseñas del producto
        public List<ResenaDto> Resenas { get; set; } = new();

        // --- Propiedades calculadas (no vienen del backend) ---

        /// <summary>Indica si el producto tiene stock disponible.</summary>
        public bool EnStock => Stock > 0;

        /// <summary>Retorna la URL de la imagen principal, o un placeholder si no hay imagen.</summary>
        public string ImagenPrincipal =>
            Imagenes?.FirstOrDefault(i => i.EsPrincipal)?.Url
            ?? Imagenes?.FirstOrDefault()?.Url
            ?? "https://images.unsplash.com/photo-1608889175123-8ee362201f81?w=800";

        /// <summary>Nombre de la categoría como texto plano para mostrar en UI.</summary>
        public string CategoriaNombre => Categoria?.Nombre ?? "Sin categoría";

        /// <summary>
        /// Precio original simulado para efecto visual de descuento (+25% sobre el precio real).
        /// El backend no tiene campo de descuento, se genera visualmente.
        /// </summary>
        public decimal PrecioOriginalSimulado => Math.Round(Precio * 1.25m, 2);

        /// <summary>Badge de descuento visual (estático para efecto gamer).</summary>
        public int DescuentoSimulado => 20;
    }

    /// <summary>DTO de Categoría tal como viene del backend.</summary>
    public class CategoriaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }

    /// <summary>DTO de imagen de producto.</summary>
    public class ImagenProductoDto
    {
        public int Id { get; set; }
        public string Url { get; set; } = string.Empty;
        public bool EsPrincipal { get; set; }
        public int ProductoId { get; set; }
    }

    /// <summary>DTO de reseña de producto.</summary>
    public class ResenaDto
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public int UsuarioId { get; set; }
        public int Calificacion { get; set; }
        public string Comentario { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
    }
    public class RespuestaIADto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal PrecioSugerido { get; set; }
        public string CategoriaSugerida { get; set; } = string.Empty;
        public string Genero { get; set; } = string.Empty;
        public string RequisitosSistema { get; set; } = string.Empty;
        public bool EsDigital { get; set; }
        public List<string> ImagenesSugeridas { get; set; } = new();
    }
}
