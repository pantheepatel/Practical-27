using DotNet6Practical.Data;
using DotNet6Practical.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace DotNet6Practical.Controllers
{
    [ApiController]
    [Route("[controller]")]

    // use primary constructor for .net 9 version
    public class EmployeeController : ControllerBase
    {
        private readonly Practical27Context _context;
        public EmployeeController(Practical27Context context)
        {
            _context = context;
        }

        [HttpGet]
        public List<Employee> GetEmployees()
        {
            return _context.Employees.ToList();
        }

        [HttpGet("{id?}")]
        public IActionResult GetEmployee(int? id)
        {
            if (id.HasValue)
            {
                var employee = _context.Employees.Find(id);
                if (employee == null)
                {
                    return NotFound($"Employee with id {id.HasValue} can not be found");
                }
                else if (employee.EmployeeStatus != "Active")
                {
                    return BadRequest("Sorry, this user is inactive. Can not show details.");
                }
                return Ok(employee);
            }
            else
            {
                return Ok(_context.Employees.ToList());
            }
        }

        [HttpPost]
        public IActionResult AddEmployee([FromBody] EmployeeDto employeeDto)
        {
            var employee = new Employee
            {
                EmployeeName = employeeDto.EmployeeName,
                Salary = employeeDto.Salary,
                DepartmentId = employeeDto.DepartmentId,
                EmployeeEmail = employeeDto.EmployeeEmail,
                JoiningDate = DateTime.Now,
                EmployeeStatus = "Active"
            };
            try
            {
                _context.Employees.Add(employee);
                _context.SaveChanges();
                return Ok(employee);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error adding employee", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateEmployee(int id, [FromBody] EmployeeDto employeeDto)
        {
            var employee = _context.Employees.Find(id);
            if (employee == null)
            {
                return NotFound($"Employee with id {id} can not be found");
            }
            else if (employee.EmployeeStatus != "Active")
            {
                return BadRequest("Sorry, this user is inactive. Can not update details.");
            }
            employee.EmployeeName = employeeDto.EmployeeName ?? employee.EmployeeName;
            employee.Salary = employeeDto.Salary ?? employee.Salary;
            employee.DepartmentId = employeeDto.DepartmentId ?? employee.DepartmentId;
            employee.EmployeeEmail = employeeDto.EmployeeEmail ?? employee.EmployeeEmail;
            try
            {
                _context.SaveChanges();
                return Ok(employee);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error updating employee", error = ex.Message });
            }
        }
    }
}
