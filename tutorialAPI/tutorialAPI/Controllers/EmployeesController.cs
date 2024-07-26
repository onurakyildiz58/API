using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using tutorialAPI.Data;
using tutorialAPI.Models.Dto;
using tutorialAPI.Models.Entities;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace tutorialAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {           
        private readonly ApplicationDBContext dBContext;
        private readonly IWebHostEnvironment env;

        public EmployeesController(ApplicationDBContext dBContext, IWebHostEnvironment env) 
        {
            this.dBContext = dBContext;
            this.env = env;
        }

        [HttpGet]
        [Route("get/")]
        public IActionResult Get()
        {
            var result = dBContext.Employees.ToList();

            return Ok(result);
        }

        [HttpGet]
        [Route("getUser/{id:guid}")]
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
        [Route("add/")]
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
        [Route("update/{id:guid}")]
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
        [Route("delete/{id:guid}")]
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

        [HttpPost]
        [Route("savefile/")]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string fileName = postedFile.FileName;
                var physicalPath = env.ContentRootPath + "/images/" + fileName;

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                return new JsonResult(fileName);
            }
            catch (Exception)
            {
                return new JsonResult("anonymous.png");
            }
        }

        [HttpGet]
        [Route("getAllDeps/")]
        public IActionResult GetDeps()
        {
            var result = dBContext.Departments.ToList();

            return Ok(result);
        }
    }
}
