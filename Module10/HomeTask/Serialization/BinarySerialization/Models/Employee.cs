using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerialization.Models
{
    [MessagePackObject]
    public class Employee
    {
        [Key(0)]
        public string EmployeeName { get; set; }
    }
}
