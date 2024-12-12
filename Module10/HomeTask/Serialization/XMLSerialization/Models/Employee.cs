using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XMLSerialization.Models
{
    public class Employee
    {
        [XmlAttribute("Name")]
        public string EmployeeName { get; set; }
    }
}
