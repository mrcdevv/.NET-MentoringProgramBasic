using System;
using System.Collections.Generic;
using System.Linq;
using Task1.DoNotChange;

namespace Task1
{
    public static class LinqTask
    {
        public static IEnumerable<Customer> Linq1(IEnumerable<Customer> customers, decimal limit)
        {
            return customers.Where(c => c.Orders.Sum(o => o.Total) > limit);
        }

        public static IEnumerable<(Customer customer, IEnumerable<Supplier> suppliers)> Linq2(
            IEnumerable<Customer> customers,
            IEnumerable<Supplier> suppliers
        )
        {
            if (customers == null)
                throw new ArgumentNullException(nameof(customers));
            if (suppliers == null)
                throw new ArgumentNullException(nameof(suppliers));

            return customers.Select(customer =>
                (
                    customer,
                    suppliers: suppliers.Where(supplier =>
                        supplier.Country == customer.Country && supplier.City == customer.City)
                )
            );
        }

        public static IEnumerable<(Customer customer, IEnumerable<Supplier> suppliers)> Linq2UsingGroup(
            IEnumerable<Customer> customers,
            IEnumerable<Supplier> suppliers
        )
        {
            if (customers == null)
                throw new ArgumentNullException(nameof(customers));
            if (suppliers == null)
                throw new ArgumentNullException(nameof(suppliers));

            var groupedSuppliers = suppliers.GroupBy(supplier => new { supplier.Country, supplier.City })
                                             .ToDictionary(group => group.Key, group => group.AsEnumerable());

            return customers.Select(customer =>
                (
                    customer,
                    suppliers: groupedSuppliers.TryGetValue(
                        new { customer.Country, customer.City },
                        out var matchingSuppliers
                    ) ? matchingSuppliers : Enumerable.Empty<Supplier>()
                )
            );
        }

        public static IEnumerable<Customer> Linq3(IEnumerable<Customer> customers, decimal limit)
        {
            if (customers == null)
                throw new ArgumentNullException(nameof(customers));

            return customers
                .Where(customer => (customer.Orders?.Sum(order => order.Total) ?? 0) > limit)
                .OrderBy(customer => customer.Orders == null || customer.Orders.Length == 0 ? int.MaxValue : customer.Orders.Min(o => o.OrderDate).Year)
                .ThenBy(customer => customer.Orders == null || customer.Orders.Length == 0 ? int.MaxValue : customer.Orders.Min(o => o.OrderDate).Month)
                .ThenByDescending(customer => customer.Orders?.Sum(o => o.Total) ?? 0)
                .ThenBy(customer => customer.CompanyName);
        }

        public static IEnumerable<(Customer customer, DateTime dateOfEntry)> Linq4(
            IEnumerable<Customer> customers
        )
        {
            if (customers == null)
                throw new ArgumentNullException(nameof(customers));

            return customers
                .Where(c => c.Orders != null && c.Orders.Any())
                .Select(customer => (
                    customer,
                    dateOfEntry: customer.Orders.Min(order => order.OrderDate)
                ));
        }

        public static IEnumerable<(Customer customer, DateTime dateOfEntry)> Linq5(
            IEnumerable<Customer> customers
        )
        {
            if (customers == null)
                throw new ArgumentNullException(nameof(customers));

            return customers
                .Where(c => c.Orders != null && c.Orders.Any())
                .Select(customer => (
                    customer,
                    dateOfEntry: customer.Orders.Min(order => order.OrderDate)
                ))
                .OrderBy(tuple => tuple.dateOfEntry.Year)
                .ThenBy(tuple => tuple.dateOfEntry.Month)
                .ThenByDescending(tuple => tuple.customer.Orders.Sum(o => o.Total))
                .ThenBy(tuple => tuple.customer.CompanyName);
        }

        public static IEnumerable<Customer> Linq6(IEnumerable<Customer> customers)
        {
            if (customers == null)
                throw new ArgumentNullException(nameof(customers));

            return customers.Where(c =>
                (c.PostalCode?.All(ch => !char.IsDigit(ch)) == true) ||
                string.IsNullOrWhiteSpace(c.Region) ||
                (c.Phone != null && !c.Phone.Contains("("))
            );
        }

        public static IEnumerable<Linq7CategoryGroup> Linq7(IEnumerable<Product> products)
        {
            if (products == null)
                throw new ArgumentNullException(nameof(products));

            return products
                .GroupBy(p => p.Category)
                .Select(categoryGroup => new Linq7CategoryGroup
                {
                    Category = categoryGroup.Key,
                    UnitsInStockGroup = categoryGroup
                        .GroupBy(p => p.UnitsInStock)
                        .Select(stockGroup => new Linq7UnitsInStockGroup
                        {
                            UnitsInStock = stockGroup.Key,
                            Prices = stockGroup.Select(p => p.UnitPrice).OrderBy(price => price)
                        })
                        .OrderByDescending(g => g.UnitsInStock)
                });
        }

        public static IEnumerable<(decimal category, IEnumerable<Product> products)> Linq8(
            IEnumerable<Product> products,
            decimal cheap,
            decimal middle,
            decimal expensive
        )
        {
            if (products == null)
                throw new ArgumentNullException(nameof(products));

            return new[]
            {
                (category: cheap, products: products.Where(p => p.UnitPrice <= cheap)),
                (category: middle, products: products.Where(p => p.UnitPrice > cheap && p.UnitPrice <= middle)),
                (category: expensive, products: products.Where(p => p.UnitPrice > middle && p.UnitPrice <= expensive))
            };
        }

        public static IEnumerable<(string city, int averageIncome, int averageIntensity)> Linq9(
            IEnumerable<Customer> customers
        )
        {
            if (customers == null)
                throw new ArgumentNullException(nameof(customers));

            return customers.GroupBy(c => c.City).Select(group =>
            {
                decimal totalIncome = group.Sum(c => c.Orders.Sum(o => o.Total));
                int averageIncome = group.Any() ? (int)(totalIncome / group.Count()) : 0;
                var customersWithOrders = group.Where(c => c.Orders.Any());
                int averageIntensity = customersWithOrders.Any() ? (int)customersWithOrders.Average(c => c.Orders.Length) : 0;

                return (group.Key, averageIncome, averageIntensity);
            });

        }

        public static string Linq10(IEnumerable<Supplier> suppliers)
        {
            if (suppliers == null)
                throw new ArgumentNullException(nameof(suppliers));

            return string.Concat(
                suppliers
                    .Select(s => s.Country)
                    .Distinct()
                    .OrderBy(c => c.Length)
                    .ThenBy(c => c)
            );
        }
    }
}