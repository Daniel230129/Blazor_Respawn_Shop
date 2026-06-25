using System;
using System.Collections.Generic;
using System.Linq;
using Blazor_Respawn_Shop.Models;
using Microsoft.Extensions.Logging; // <-- 1. Para el ILogger
using Microsoft.Extensions.Configuration; // <-- 2. Para el IConfiguration

namespace Blazor_Respawn_Shop.Services
{
    public class CartItem
    {
        public ProductoDto Producto { get; set; } = new();
        public int Cantidad { get; set; } = 1;
        public decimal Subtotal => Producto.Precio * Cantidad;
    }

    public class CartService
    {
        private readonly List<CartItem> _items = new();
        private readonly ILogger<CartService> _logger;
        private readonly IConfiguration _config;

        public CartService(ILogger<CartService> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;

            _logger.LogInformation(" CartService de Blazor inicializado correctamente.");
        }

        public IReadOnlyList<CartItem> Items => _items;

        public event Action? OnChange;

        public void AñadirAlCarrito(ProductoDto producto)
        {
            _logger.LogInformation("Intentando añadir al carrito: {Nombre} (Stock disponible: {Stock})", producto.Nombre, producto.Stock);

            var itemExistente = _items.FirstOrDefault(i => i.Producto.Id == producto.Id);

            if (itemExistente != null)
            {
                if (itemExistente.Cantidad < producto.Stock)
                {
                    itemExistente.Cantidad++;
                    _logger.LogInformation("Cantidad incrementada para {Nombre}. Total en carrito: {Cantidad}", producto.Nombre, itemExistente.Cantidad);
                    NotificarCambio();
                }
                else
                {
                    _logger.LogWarning(" No se pudo agregar más unidades. Límite de stock alcanzado para: {Nombre}", producto.Nombre);
                }
            }
            else
            {
                if (producto.Stock > 0)
                {
                    _items.Add(new CartItem { Producto = producto, Cantidad = 1 });
                    _logger.LogInformation(" Producto añadido al carrito por primera vez: {Nombre}", producto.Nombre);
                    NotificarCambio();
                }
                else
                {
                    _logger.LogWarning(" Intento de añadir producto sin stock: {Nombre}", producto.Nombre);
                }
            }
        }

        public void RestarDelCarrito(int productoId)
        {
            var itemExistente = _items.FirstOrDefault(i => i.Producto.Id == productoId);
            if (itemExistente != null)
            {
                itemExistente.Cantidad--;

                if (itemExistente.Cantidad <= 0)
                {
                    _items.Remove(itemExistente);
                }
                NotificarCambio();
            }
        }

        public void EliminarTotalmente(int productoId)
        {
            var item = _items.FirstOrDefault(i => i.Producto.Id == productoId);
            if (item != null)
            {
                _items.Remove(item);
                NotificarCambio();
            }
        }

        public void VaciarCarrito()
        {
            _items.Clear();
            _logger.LogInformation("El carrito ha sido vaciado completamente tras una operación exitosa.");
            NotificarCambio();
        }

        public int ContadorProductos => _items.Sum(i => i.Cantidad);
        public decimal TotalCarrito => _items.Sum(i => i.Subtotal);

        private void NotificarCambio() => OnChange?.Invoke();
    }
}