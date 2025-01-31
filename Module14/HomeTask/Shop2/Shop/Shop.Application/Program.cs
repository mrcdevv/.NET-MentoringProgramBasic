﻿using Microsoft.Extensions.DependencyInjection;
using Shop.Core.Data;
using Shop.Core.Models;
using Shop.Core.Repositories;
using Shop.Core.Services;
using System;
using System.Threading.Tasks;

namespace Shop.Application
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string connectionString = @"Server=DESKTOP-SO490Q0\SQLEXPRESS;Database=Store;Integrated Security=True;TrustServerCertificate=True;";
            var services = new ServiceCollection();
            ConfigureServices(services, connectionString);
            var serviceProvider = services.BuildServiceProvider();

            var dbInitializer = serviceProvider.GetRequiredService<DatabaseHelper>();
            var unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();
            var productService = serviceProvider.GetRequiredService<ProductService>();
            var orderService = serviceProvider.GetRequiredService<OrderService>();

            try
            {
                await DbInitializer.CleanDatabaseAsync(connectionString);
                Console.WriteLine("Database cleaned successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error cleaning database: {ex.Message}");
                return;
            }

            try
            {
                await DbInitializer.InitializeAsync(connectionString);
                Console.WriteLine("Database initialized successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing database: {ex.Message}");
                return;
            }

            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            try
            {
                // Get all products
                var products = await productService.GetAllAsync(cancellationToken);
                Console.WriteLine("List of products:");
                foreach (var product in products)
                {
                    Console.WriteLine($"ID: {product.Id}, Name: {product.Name}, Description: {product.Description}");
                }

                // Get an order by ID
                var order = await orderService.GetByIdAsync(1, cancellationToken);
                Console.WriteLine("\nOrder retrieved by ID 1:");
                Console.WriteLine($"ID: {order.Id}, Status: {order.Status}, Created Date: {order.CreatedDate}");

                // Get all orders
                var orders = await orderService.GetAllAsync(cancellationToken);
                Console.WriteLine("\nList of orders:");
                foreach (var o in orders)
                {
                    Console.WriteLine($"ID: {o.Id}, Status: {o.Status}, Created Date: {o.CreatedDate}");
                }

                // Get filtered orders by status "InProgress"
                var filteredOrders = await orderService.GetOrdersFilteredAsync(null, null, OrderStatus.InProgress, null, cancellationToken);
                Console.WriteLine("\nOrders in progress:");
                foreach (var o in filteredOrders)
                {
                    Console.WriteLine($"ID: {o.Id}, Status: {o.Status}");
                }

                // Get orders that include the product with ID 1 (Laptop)
                var ordersWithLaptop = await orderService.GetOrdersFilteredAsync(null, null, null, 1, cancellationToken);
                Console.WriteLine("\nOrders that include Laptop (ID 1):");
                foreach (var o in ordersWithLaptop)
                {
                    Console.WriteLine($"ID: {o.Id}, Status: {o.Status}");
                }

                // Delete an order
                await orderService.DeleteAsync(3, cancellationToken);
                Console.WriteLine("\nOrder with ID 3 deleted.");

                // Get remaining orders
                var remainingOrders = await orderService.GetAllAsync(cancellationToken);
                Console.WriteLine("\nRemaining orders:");
                foreach (var o in remainingOrders)
                {
                    Console.WriteLine($"ID: {o.Id}, Status: {o.Status}");
                }

                // Delete a product
                await productService.DeleteAsync(1, cancellationToken);
                Console.WriteLine("\nProduct with ID 1 deleted.");

                var remainingProducts = await productService.GetAllAsync(cancellationToken);
                Console.WriteLine("\nRemaining products:");
                foreach (var product in remainingProducts)
                {
                    Console.WriteLine($"ID: {product.Id}, Name: {product.Name}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private static void ConfigureServices(IServiceCollection services, string strConex)
        {
            services.AddScoped<DatabaseHelper>(provider =>
                new DatabaseHelper(strConex));

            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<OrderService>();
            services.AddScoped<ProductService>();
        }
    }
}