using JsonSerialization.Models;
using System.Text.Json;
using System.Xml.Serialization;

namespace JsonSerialization
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var department = new Department
            {
                DepartmentName = "IT Department",
                Employees = new List<Employee>
                {
                    new Employee { EmployeeName = "Alex" },
                    new Employee { EmployeeName = "Pedro" }
                }
            };

            string filePath = "department.json";

            // Serialize
            await using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                await JsonSerializer.SerializeAsync(fileStream, department, new JsonSerializerOptions { WriteIndented = true });
            }
            Console.WriteLine("Department serialized to JSON format.");

            // Deserialize
            await using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                Department deserialized = await JsonSerializer.DeserializeAsync<Department>(fileStream);
                Console.WriteLine($"Deserialized Department: {deserialized.DepartmentName}");
            }
        }
    }
}
