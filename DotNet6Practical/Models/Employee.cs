using System;
using System.Collections.Generic;

namespace DotNet6Practical.Models
{
    public partial class Employee
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = null!;
        public decimal? Salary { get; set; }
        public int? DepartmentId { get; set; }
        public string? EmployeeEmail { get; set; }
        public DateTime? JoiningDate { get; set; }
        public string? EmployeeStatus { get; set; }

        public virtual Department? Department { get; set; }
    }
}
