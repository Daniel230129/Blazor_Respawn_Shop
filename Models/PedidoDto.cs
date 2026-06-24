namespace Blazor_Respawn_Shop.Models
{
    public class PedidoDto
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public DateTime FechaPedido { get; set; }
        public decimal TotalPagado { get; set; }
        public string Estado { get; set; } = string.Empty;
    }
}