using System.Xml.Serialization;
using XMLSerialization.Models;

namespace XMLSerialization
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

            string filePath = "department.xml";

            // Serialize
            XmlSerializer serializer = new(typeof(Department));
            await using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                using (var writer = new StreamWriter(fileStream))
                {
                    serializer.Serialize(writer, department);
                }
            }
            Console.WriteLine("Department serialized to XML format.");

            // Deserialize
            await using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (var reader = new StreamReader(fileStream))
                {
                    Department deserialized = (Department)serializer.Deserialize(reader);
                    Console.WriteLine($"Deserialized Department: {deserialized.DepartmentName}");
                }
            }
        }
    }
}
