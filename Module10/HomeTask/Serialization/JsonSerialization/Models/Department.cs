using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace JsonSerialization.Models
{
    public class Department
    {
        [JsonPropertyName("department_name")]
        public string DepartmentName { get; set; }

        [JsonPropertyName("employees")]
        public IList<Employee> Employees { get; set; }
    }
}
