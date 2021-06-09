using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SubReportInRDLC.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

    }
}