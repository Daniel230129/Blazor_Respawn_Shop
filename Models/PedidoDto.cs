namespace Blazor_Respawn_Shop.Models
{
    public class PedidoDto
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public DateTime FechaPedido { get; set; }

        // 1. Reemplazamos el string por el objeto anidado para que haga "match" con el backend
        public UsuarioDto? Usuario { get; set; }

        public decimal TotalPagado { get; set; }
        public string Estado { get; set; } = string.Empty;

        public List<DetallePedidoDto> Detalles { get; set; } = new();
    }

    // 2. Agregas esta pequeña clase ahí mismo para atrapar el nombre
    public class UsuarioDto
    {
        // OJO: Si tu columna en la base de datos se llama "Nombres", "Username" 
        // o "FullName", tienes que escribirlo exactamente igual aquí abajo:
        public string Nombre { get; set; } = string.Empty;
    }

    public class DetallePedidoDto
    {
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public ProductoDto? Producto { get; set; }
    }
}