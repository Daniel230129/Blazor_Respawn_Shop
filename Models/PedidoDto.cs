namespace Blazor_Respawn_Shop.Models
{
    public class PedidoDto
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public DateTime FechaPedido { get; set; }
        public decimal TotalPagado { get; set; }
        public string Estado { get; set; } = string.Empty;

        // ¡EL INGREDIENTE SECRETO PARA VER LOS JUEGOS!
        public List<DetallePedidoDto> Detalles { get; set; } = new();
    }

    public class DetallePedidoDto
    {
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }

        // Si el backend nos manda los datos del juego anidados, caen aquí:
        public ProductoDto? Producto { get; set; }
    }
}