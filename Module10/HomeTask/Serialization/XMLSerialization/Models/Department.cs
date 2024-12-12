using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XMLSerialization.Models
{
    public class Department
    {
        [XmlAttribute("DepartmentName")]
        public string DepartmentName { get; set; }

        [XmlArray("Employees")]
        [XmlArrayItem("Employee")]
        public List<Employee> Employees { get; set; }
    }
}
