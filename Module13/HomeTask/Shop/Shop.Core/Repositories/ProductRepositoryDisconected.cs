using Microsoft.Data.SqlClient;
using Shop.Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Core.Repositories
{
    public class ProductRepositoryDisconnected : IProductRepository
    {
        private readonly string _connectionString;

        public ProductRepositoryDisconnected(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Product> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var dataSet = new DataSet();
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT Id, Name, Description, Weight, Height, Width, Length FROM Product WHERE Id = @Id";
                var adapter = new SqlDataAdapter(query, connection);
                adapter.SelectCommand.Parameters.AddWithValue("@Id", id);

                await Task.Run(() => adapter.Fill(dataSet, "Product"), cancellationToken);
            }

            if (dataSet.Tables["Product"].Rows.Count > 0)
            {
                var row = dataSet.Tables["Product"].Rows[0];
                return new Product
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Name = row["Name"].ToString(),
                    Description = row["Description"].ToString(),
                    Weight = Convert.ToDecimal(row["Weight"]),
                    Height = Convert.ToDecimal(row["Height"]),
                    Width = Convert.ToDecimal(row["Width"]),
                    Length = Convert.ToDecimal(row["Length"])
                };
            }
            return null;
        }

        public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var products = new List<Product>();
            var dataSet = new DataSet();
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT Id, Name, Description, Weight, Height, Width, Length FROM Product";
                var adapter = new SqlDataAdapter(query, connection);

                await Task.Run(() => adapter.Fill(dataSet, "Product"), cancellationToken);
            }

            foreach (DataRow row in dataSet.Tables["Product"].Rows)
            {
                products.Add(new Product
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Name = row["Name"].ToString(),
                    Description = row["Description"].ToString(),
                    Weight = Convert.ToDecimal(row["Weight"]),
                    Height = Convert.ToDecimal(row["Height"]),
                    Width = Convert.ToDecimal(row["Width"]),
                    Length = Convert.ToDecimal(row["Length"])
                });
            }
            return products;
        }

        public async Task AddAsync(Product product, CancellationToken cancellationToken = default)
        {
            var dataSet = new DataSet();
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT Id, Name, Description, Weight, Height, Width, Length FROM Product";
                var adapter = new SqlDataAdapter(query, connection);
                var builder = new SqlCommandBuilder(adapter);

                await Task.Run(() => adapter.Fill(dataSet, "Product"), cancellationToken);

                var newRow = dataSet.Tables["Product"].NewRow();
                newRow["Id"] = product.Id;
                newRow["Name"] = product.Name;
                newRow["Description"] = product.Description;
                newRow["Weight"] = product.Weight;
                newRow["Height"] = product.Height;
                newRow["Width"] = product.Width;
                newRow["Length"] = product.Length;
                dataSet.Tables["Product"].Rows.Add(newRow);

                await Task.Run(() => adapter.Update(dataSet, "Product"), cancellationToken);
            }
        }

        public async Task UpdateAsync(Product product, CancellationToken cancellationToken = default)
        {
            var dataSet = new DataSet();
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT Id, Name, Description, Weight, Height, Width, Length FROM Product WHERE Id = @Id";
                var adapter = new SqlDataAdapter(query, connection);
                adapter.SelectCommand.Parameters.AddWithValue("@Id", product.Id);
                var builder = new SqlCommandBuilder(adapter);

                await Task.Run(() => adapter.Fill(dataSet, "Product"), cancellationToken);

                if (dataSet.Tables["Product"].Rows.Count > 0)
                {
                    var row = dataSet.Tables["Product"].Rows[0];
                    row["Name"] = product.Name;
                    row["Description"] = product.Description;
                    row["Weight"] = product.Weight;
                    row["Height"] = product.Height;
                    row["Width"] = product.Width;
                    row["Length"] = product.Length;

                    await Task.Run(() => adapter.Update(dataSet, "Product"), cancellationToken);
                }
            }
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var deleteOrderProductCommand = new SqlCommand(
                            "DELETE FROM OrderProduct WHERE ProductId = @ProductId",
                            connection, transaction);
                        deleteOrderProductCommand.Parameters.AddWithValue("@ProductId", id);
                        await deleteOrderProductCommand.ExecuteNonQueryAsync(cancellationToken);

                        var deleteProductCommand = new SqlCommand(
                            "DELETE FROM Product WHERE Id = @Id",
                            connection, transaction);
                        deleteProductCommand.Parameters.AddWithValue("@Id", id);
                        await deleteProductCommand.ExecuteNonQueryAsync(cancellationToken);

                        await transaction.CommitAsync(cancellationToken);
                    }
                    catch
                    {
                        await transaction.RollbackAsync(cancellationToken);
                        throw;
                    }
                }
            }
        }
    }
}
