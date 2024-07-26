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
    public class DepartmentsController : ControllerBase
    {
        private readonly ApplicationDBContext dBContext;

        public DepartmentsController(ApplicationDBContext dBContext)
        {
            this.dBContext = dBContext;
        }

        [HttpGet]
        [Route("get")]
        public IActionResult Get()
        {
            var result = dBContext.Departments.ToList();

            return Ok(result);
        }

        [HttpGet]
        [Route("getDepartment/{id:guid}")]
        public IActionResult GetByID(Guid id)
        {
            var result = dBContext.Departments.Find(id);

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        [Route("add")]
        public IActionResult Add(AddDepartmentDto addDepartmentDto)
        {
            var departmentEntitiy = new Department()
            {
                name = addDepartmentDto.name
            };

            dBContext.Departments.Add(departmentEntitiy);
            dBContext.SaveChanges();

            return Ok(departmentEntitiy);
        }

        [HttpPut]
        [Route("update/{id:guid}")]                            
        public IActionResult Put(Guid id, AddDepartmentDto addDepartmentDto)
        {
            var result = dBContext.Departments.Find(id);

            if (result is null)
            {
                return NotFound();
            }

            result.name = addDepartmentDto.name;
            dBContext.SaveChanges();

            return Ok(result);

        }

        [HttpDelete]
        [Route("delete/{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var result = dBContext.Departments.Find(id);

            if (result is null)
            {
                return NotFound();
            }

            dBContext.Departments.Remove(result);
            dBContext.SaveChanges();

            return Ok(result);
        }
    }
}
