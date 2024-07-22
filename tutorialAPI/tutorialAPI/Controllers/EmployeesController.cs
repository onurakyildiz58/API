using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using tutorialAPI.Data;
using tutorialAPI.Models.Dto;
using tutorialAPI.Models.Entities;

namespace tutorialAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {           
        private readonly ApplicationDBContext dBContext;

        public EmployeesController(ApplicationDBContext dBContext) 
        {
            this.dBContext = dBContext;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var result = dBContext.Employees.ToList();

            return Ok(result);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public IActionResult GetByID(Guid id)
        {
            var result = dBContext.Employees.Find(id);

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        public IActionResult Add(AddEmployeeDto addEmployeeDto)
        {
            var employeeEntitiy = new Employee()
            {
                depID = addEmployeeDto.depID,
                fullname = addEmployeeDto.fullname,
                created_at = addEmployeeDto.created_at,
                imagePath = addEmployeeDto.imagePath
            };

            dBContext.Employees.Add(employeeEntitiy);
            dBContext.SaveChanges();

            return Ok(employeeEntitiy);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public IActionResult Put(Guid id, AddEmployeeDto addEmployeeDto)
        {
            var result =  dBContext.Employees.Find(id);

            if (result is null)
            {
                return NotFound();
            }

            result.depID = addEmployeeDto.depID;
            result.fullname = addEmployeeDto.fullname;
            result.created_at = addEmployeeDto.created_at;
            result.imagePath = addEmployeeDto.imagePath;
            dBContext.SaveChanges();

            return Ok(result);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var result = dBContext.Employees.Find(id);

            if (result is null)
            {
                return NotFound();
            }

            dBContext.Employees.Remove(result);
            dBContext.SaveChanges();

            return Ok(result);
        }
    }
}
