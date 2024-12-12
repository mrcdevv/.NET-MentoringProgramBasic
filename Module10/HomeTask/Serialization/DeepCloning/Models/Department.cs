using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerialization.Models
{
    [MessagePackObject]
    public class Department
    {
        [Key(0)]
        public string DepartmentName { get; set; }
        [Key(1)]
        public IList<Employee> Employees { get; set; }
        
        public override string ToString()
        {
            return $"Department: {DepartmentName}, Employees: [ {string.Join(", ", Employees)} ]";
        }
    }
}
