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

        public async Task<Product> GetByIdAsync(int id)
        {
            var dataSet = new DataSet();
            using (var connection = new SqlConnection(_connectionString))
            {
                var adapter = new SqlDataAdapter("SELECT * FROM Product WHERE Id = @Id", connection);
                adapter.SelectCommand.Parameters.AddWithValue("@Id", id);
                await Task.Run(() => adapter.Fill(dataSet, "Product"));
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

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var products = new List<Product>();
            var dataSet = new DataSet();
            using (var connection = new SqlConnection(_connectionString))
            {
                var adapter = new SqlDataAdapter("SELECT * FROM Product", connection);
                await Task.Run(() => adapter.Fill(dataSet, "Product"));
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

        public async Task AddAsync(Product product)
        {
            var dataSet = new DataSet();
            using (var connection = new SqlConnection(_connectionString))
            {
                var adapter = new SqlDataAdapter("SELECT * FROM Product", connection);
                var builder = new SqlCommandBuilder(adapter);
                await Task.Run(() => adapter.Fill(dataSet, "Product"));

                var newRow = dataSet.Tables["Product"].NewRow();
                newRow["Id"] = product.Id;
                newRow["Name"] = product.Name;
                newRow["Description"] = product.Description;
                newRow["Weight"] = product.Weight;
                newRow["Height"] = product.Height;
                newRow["Width"] = product.Width;
                newRow["Length"] = product.Length;
                dataSet.Tables["Product"].Rows.Add(newRow);

                await Task.Run(() => adapter.Update(dataSet, "Product"));
            }
        }

        public async Task UpdateAsync(Product product)
        {
            var dataSet = new DataSet();
            using (var connection = new SqlConnection(_connectionString))
            {
                var adapter = new SqlDataAdapter("SELECT * FROM Product WHERE Id = @Id", connection);
                adapter.SelectCommand.Parameters.AddWithValue("@Id", product.Id);
                var builder = new SqlCommandBuilder(adapter);
                await Task.Run(() => adapter.Fill(dataSet, "Product"));

                if (dataSet.Tables["Product"].Rows.Count > 0)
                {
                    var row = dataSet.Tables["Product"].Rows[0];
                    row["Name"] = product.Name;
                    row["Description"] = product.Description;
                    row["Weight"] = product.Weight;
                    row["Height"] = product.Height;
                    row["Width"] = product.Width;
                    row["Length"] = product.Length;

                    await Task.Run(() => adapter.Update(dataSet, "Product"));
                }
            }
        }

        public async Task DeleteAsync(int id)
        {
            var dataSet = new DataSet();
            using (var connection = new SqlConnection(_connectionString))
            {
                var deleteOrderProductCommand = new SqlCommand("DELETE FROM OrderProduct WHERE ProductId = @ProductId", connection);
                deleteOrderProductCommand.Parameters.AddWithValue("@ProductId", id);
                await deleteOrderProductCommand.ExecuteNonQueryAsync();

                var adapter = new SqlDataAdapter("SELECT * FROM Product WHERE Id = @Id", connection);
                adapter.SelectCommand.Parameters.AddWithValue("@Id", id);
                var builder = new SqlCommandBuilder(adapter);
                await Task.Run(() => adapter.Fill(dataSet, "Product"));

                if (dataSet.Tables["Product"].Rows.Count > 0)
                {
                    var row = dataSet.Tables["Product"].Rows[0];
                    row.Delete();

                    await Task.Run(() => adapter.Update(dataSet, "Product"));
                }
            }
        }
    }
}
