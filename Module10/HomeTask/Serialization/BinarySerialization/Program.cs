using BinarySerialization.Models;
using MessagePack;
using System.Runtime.Serialization.Formatters.Binary;

namespace BinarySerialization
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

            string filePath = "department.msgpack";

            // Serialization
            await using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                await MessagePackSerializer.SerializeAsync(fileStream, department);
            }
            Console.WriteLine($"Department serialized to binary format and saved at: {filePath}");

            // Deserialization
            await using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                Department deserialized = await MessagePackSerializer.DeserializeAsync<Department>(fileStream);
                Console.WriteLine($"Deserialized Department: {deserialized.DepartmentName}");
            }
        }
    }
}
