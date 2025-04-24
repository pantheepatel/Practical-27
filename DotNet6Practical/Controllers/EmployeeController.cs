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

        [HttpGet("{id}")]
        public ActionResult<Employee> GetEmployee(int id)
        {
            var employee = _context.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }
            return employee;
        }

        [HttpPost]
        public HttpResponseMessage AddEmployee(Employee employee)
        {
            try
            {
                _context.Employees.Add(employee);
                _context.SaveChanges();
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            }
            finally
            {
                _context.SaveChanges();
                //return new HttpResponseMessage(HttpStatusCode.OK);
            }
        }
    }
}
