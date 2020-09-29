using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Controllers
{
    public class DemoController : ControllerBase
    {

        // Route Params
        // GET /employees/19
        [HttpGet("/employees/{employeeId:int}")]
        public ActionResult LookupEmployee([FromRoute] int employeeId)
        {
            return Ok($"Finding employee { employeeId }");
        }

        // GET /blogs/2020/09/18
        [HttpGet("/blogs/{year:int}/{month:int:range(1,12)}/{day:int:range(1,31)}")]
        public ActionResult GetBlogPosts([FromRoute] int year, [FromRoute] int month, [FromRoute] int day)
        {
            return Ok($"Getting blogs for {year}/{month}/{day}");
        }
        // Query Strings
        // GET /employees?dept=DEV
        [HttpGet("/employees")]
        public ActionResult GetEmployees([FromQuery] string department = "All", [FromQuery] decimal minSalary = 0)
        {
            return Ok($"Returning all employees from {department} with a minumum salary of {minSalary:c}");
        }

        // Headers
        [HttpGet("/whoami")]
        public ActionResult ShowUserAgent([FromHeader(Name ="User-Agent")] string userAgent)
        {
            return Ok($"I see you are running {userAgent}");
        }

        // Entities

        [HttpPost("/employees")]
        public ActionResult Hire([FromBody] PostEmployeeCreate employeeToHire)
        {
            // Your template for doing a post to a collection.
            // Validate the data. If it is bad, send a 400 with or without details.
            return Ok($"You want to hire {employeeToHire.Name} in {employeeToHire.Department} at {employeeToHire.StartingSalary:c}");

        }

        
    }

    public class PostEmployeeCreate
    {
        public string Name { get; set; }
        public string Department { get; set; }
        public decimal StartingSalary { get; set; }
    }
}
