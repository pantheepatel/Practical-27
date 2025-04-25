namespace DotNet9Practical.Models
{
    public class EmployeeDto
    {
        public string EmployeeName { get; set; } = null!;
        public decimal? Salary { get; set; }
        public int? DepartmentId { get; set; }
        public string? EmployeeEmail { get; set; }
    }
}
