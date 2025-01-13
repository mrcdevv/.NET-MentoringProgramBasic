using Shop.Core.Data;
using Shop.Core.Models;
using Shop.Core.Repositories;
using Shop.Core.Services;

namespace Shop.Application
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string connectionString = "";

            var dbHelper = new DatabaseHelper(connectionString);
            var productRepository = new ProductRepository(dbHelper);
            var orderRepository = new OrderRepository(dbHelper);

            var productService = new ProductService(productRepository);
            var orderService = new OrderService(orderRepository);

            try
            {
                var products = await productService.GetAllAsync();
                Console.WriteLine("Lista de productos:");
                foreach (var product in products)
                {
                    Console.WriteLine($"ID: {product.Id}, Nombre: {product.Name}, Descripción: {product.Description}");
                }

                var order = await orderService.GetByIdAsync(1);
                Console.WriteLine("\nOrden obtenida por ID 1:");
                Console.WriteLine($"ID: {order.Id}, Estado: {order.Status}, Fecha de creación: {order.CreatedDate}");

                var orders = await orderService.GetAllAsync();
                Console.WriteLine("\nLista de órdenes:");
                foreach (var o in orders)
                {
                    Console.WriteLine($"ID: {o.Id}, Estado: {o.Status}, Fecha de creación: {o.CreatedDate}");
                }

                var filteredOrders = await orderService.GetOrdersFilteredAsync(null, null, OrderStatus.InProgress, null);
                Console.WriteLine("\nÓrdenes en progreso:");
                foreach (var o in filteredOrders)
                {
                    Console.WriteLine($"ID: {o.Id}, Estado: {o.Status}");
                }

                var ordersWithLaptop = await orderService.GetOrdersFilteredAsync(null, null, null, 1);
                Console.WriteLine("\nÓrdenes que incluyen Laptop (ID 1):");
                foreach (var o in ordersWithLaptop)
                {
                    Console.WriteLine($"ID: {o.Id}, Estado: {o.Status}");
                }

                await orderService.DeleteAsync(3);
                Console.WriteLine("\nOrden con ID 3 eliminada.");

                var remainingOrders = await orderService.GetAllAsync();
                Console.WriteLine("\nÓrdenes restantes:");
                foreach (var o in remainingOrders)
                {
                    Console.WriteLine($"ID: {o.Id}, Estado: {o.Status}");
                }

                await productService.DeleteAsync(1);
                Console.WriteLine("\nProducto con ID 1 eliminado.");

                var remainingProducts = await productService.GetAllAsync();
                Console.WriteLine("\nProductos restantes:");
                foreach (var product in remainingProducts)
                {
                    Console.WriteLine($"ID: {product.Id}, Nombre: {product.Name}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
