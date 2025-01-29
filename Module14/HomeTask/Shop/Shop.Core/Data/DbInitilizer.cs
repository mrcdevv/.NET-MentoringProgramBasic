using Shop.Core.Data;
using Shop.Core.Models;

public static class DbInitializer
{
    public static async Task InitializeAsync(ShopDbContext context, CancellationToken cancellationToken = default)
    {
        await context.Database.EnsureCreatedAsync(cancellationToken);

        if (!context.Products.Any())
        {
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Laptop", Description = "High-performance laptop", Weight = 2.5m, Height = 0.5m, Width = 0.3m, Length = 0.2m },
                new Product { Id = 2, Name = "Smartphone", Description = "Latest model smartphone", Weight = 0.2m, Height = 0.15m, Width = 0.07m, Length = 0.01m },
                new Product { Id = 3, Name = "Tablet", Description = "10-inch tablet", Weight = 0.5m, Height = 0.25m, Width = 0.18m, Length = 0.08m },
                new Product { Id = 4, Name = "Monitor", Description = "27-inch 4K monitor", Weight = 5.0m, Height = 0.6m, Width = 0.7m, Length = 0.2m },
                new Product { Id = 5, Name = "Keyboard", Description = "Mechanical keyboard", Weight = 1.0m, Height = 0.05m, Width = 0.4m, Length = 0.15m }
            };
            await context.Products.AddRangeAsync(products, cancellationToken);
        }

        if (!context.Orders.Any())
        {
            var orders = new List<Order>
            {
                new Order { Id = 1, Status = OrderStatus.NotStarted, CreatedDate = new DateTime(2023, 10, 1), UpdatedDate = new DateTime(2023, 10, 1) },
                new Order { Id = 2, Status = OrderStatus.InProgress, CreatedDate = new DateTime(2023, 10, 2), UpdatedDate = new DateTime(2023, 10, 2) },
                new Order { Id = 3, Status = OrderStatus.Cancelled, CreatedDate = new DateTime(2023, 10, 3), UpdatedDate = new DateTime(2023, 10, 3) },
                new Order { Id = 4, Status = OrderStatus.Done, CreatedDate = new DateTime(2023, 10, 4), UpdatedDate = new DateTime(2023, 10, 4) },
                new Order { Id = 5, Status = OrderStatus.Loading, CreatedDate = new DateTime(2023, 10, 5), UpdatedDate = new DateTime(2023, 10, 5) }
            };
            await context.Orders.AddRangeAsync(orders, cancellationToken);
        }
        if (!context.OrderProducts.Any())
        {
            var orderProducts = new List<OrderProduct>
            {
                new OrderProduct { OrderId = 1, ProductId = 1 },
                new OrderProduct { OrderId = 1, ProductId = 2 },
                new OrderProduct { OrderId = 2, ProductId = 3 },
                new OrderProduct { OrderId = 3, ProductId = 4 },
                new OrderProduct { OrderId = 4, ProductId = 5 },
                new OrderProduct { OrderId = 5, ProductId = 1 },
                new OrderProduct { OrderId = 5, ProductId = 3 }
            };
            await context.OrderProducts.AddRangeAsync(orderProducts, cancellationToken);
        }

        await context.SaveChangesAsync(cancellationToken);
    }

    public static async Task CleanDatabaseAsync(ShopDbContext context, CancellationToken cancellationToken = default)
    {
        context.OrderProducts.RemoveRange(context.OrderProducts);
        context.Orders.RemoveRange(context.Orders);
        context.Products.RemoveRange(context.Products);

        await context.SaveChangesAsync(cancellationToken);
    }
}