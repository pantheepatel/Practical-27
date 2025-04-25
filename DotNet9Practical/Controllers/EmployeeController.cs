using DotNet9Practical.Data;
using DotNet9Practical.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace DotNet9Practical.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class EmployeeController(Practical27Context _context) : ControllerBase
    {

        [HttpGet]
        public IActionResult GetEmployees()
        {
            List<Employee> employeesList = [.. _context.Employees.Where(emp => emp.EmployeeStatus == "Active")];
            if (employeesList.Count != 0)
            {
                return Ok(employeesList);
            }
            else
            {
                return NotFound("No active employees found");
            }
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
        public IActionResult UpdateEmployee(int id, [FromBody] EmployeeDto? employeeDto)
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

        [HttpDelete("{id}")]
        public IActionResult DeleteEmployee(int id)
        {
            var employee = _context.Employees.Find(id);
            if (employee == null)
            {
                return NotFound($"Employee with id {id} can not be found");
            }
            else if (employee.EmployeeStatus != "Active")
            {
                return BadRequest("This user is already inactive.");
            }
            employee.EmployeeStatus = "Inactive";
            try
            {
                _context.SaveChanges();
                return Ok(employee);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error deleting employee", error = ex.Message });
            }
        }
    }
}
