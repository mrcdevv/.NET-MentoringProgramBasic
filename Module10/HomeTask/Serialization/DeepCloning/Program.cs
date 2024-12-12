using BinarySerialization.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeepCloning
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var originalDepartment = new Department
            {
                DepartmentName = "IT Department",
                Employees = new List<Employee>
                {
                    new Employee { EmployeeName = "Alex" },
                    new Employee { EmployeeName = "Pedro" },
                    new Employee { EmployeeName = "Marco" }
                }
            };

            Console.WriteLine("Original Department:");
            Console.WriteLine(originalDepartment);

            var clonedDepartment = await DeepCloningService.DeepCloneAsync(originalDepartment);

            clonedDepartment.DepartmentName = "HR Department";
            clonedDepartment.Employees[0].EmployeeName = "David";
            clonedDepartment.Employees[2].EmployeeName = "Luca";

            Console.WriteLine("\nModified Cloned Department:");
            Console.WriteLine(clonedDepartment);

            Console.WriteLine("\nOriginal Department after cloning:");
            Console.WriteLine(originalDepartment);
        }
    }
}
