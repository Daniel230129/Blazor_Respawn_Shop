using System;
using System.Collections.Generic;
using System.Linq;
using Blazor_Respawn_Shop.Models;

namespace Blazor_Respawn_Shop.Services
{
    // El molde nuevo que entiende de Productos + Cantidades
    public class CartItem
    {
        public ProductoDto Producto { get; set; } = new();
        public int Cantidad { get; set; } = 1;
        public decimal Subtotal => Producto.Precio * Cantidad;
    }

    public class CartService
    {
        private readonly List<CartItem> _items = new();

        public IReadOnlyList<CartItem> Items => _items;

        public event Action? OnChange;

        // Añadir (o Sumar si ya existe)
        public void AñadirAlCarrito(ProductoDto producto)
        {
            var itemExistente = _items.FirstOrDefault(i => i.Producto.Id == producto.Id);
            if (itemExistente != null)
            {
                itemExistente.Cantidad++;
            }
            else
            {
                _items.Add(new CartItem { Producto = producto, Cantidad = 1 });
            }
            NotificarCambio();
        }

        // Restar (y borrar si llega a cero)
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

        // Eliminar por completo sin importar la cantidad
        public void EliminarTotalmente(int productoId)
        {
            var item = _items.FirstOrDefault(i => i.Producto.Id == productoId);
            if (item != null)
            {
                _items.Remove(item);
                NotificarCambio();
            }
        }

        // Variables matemáticas listas para usar en la pantalla
        public int ContadorProductos => _items.Sum(i => i.Cantidad);
        public decimal TotalCarrito => _items.Sum(i => i.Subtotal);

        private void NotificarCambio() => OnChange?.Invoke();
    }
}