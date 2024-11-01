using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using WnT.API.CustomActionFilter;
using WnT.API.Models.Domain;
using WnT.API.Models.DTO.region;
using WnT.API.Repo.region;

namespace WnT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly IRegionRepo regionRepo;
        private readonly IMapper mapper;
        private readonly ILogger logger;

        public RegionsController(IRegionRepo regionRepo, IMapper mapper, ILogger<RegionsController>  logger)
        {
            this.regionRepo = regionRepo;
            this.mapper = mapper;
            this.logger = logger;
        }

        /*
          DTOs: Used for client responses to define the data shape needed by the client.
          Domain Models: Used for database entities and data fetching/saving operations.
          - Fetching: Retrieve data as domain models from the database, then map to DTOs for client responses.
          - Saving: Convert DTOs to domain models before saving to the database; map saved data back to DTOs if needed.
        */

        [HttpGet]
        [Route("getall")]
        //[Authorize(Roles = "user, admin")]
        public async Task<IActionResult> GetAll()
        {
            logger.LogInformation("regions get all method was invoked");
            var result = await regionRepo.GetAllAsync();

            // Map each domain model to a DTO for client response
            var regionDTO = mapper.Map<List<RegionDTO>>(result);
           
            logger.LogInformation($"finished regions get all method{JsonSerializer.Serialize(regionDTO)}");
            return Ok(regionDTO);
        }

        [HttpGet]
        [Route("getRegionById/{Id:Guid}")]
        [Authorize(Roles = "user,admin")]
        public async Task<IActionResult> GetByID([FromRoute] Guid Id)
        {
            var result = await regionRepo.GetByIdAsync(Id);

            if (result is null)
            {
                return NotFound();
            }
            
            var regionDTO = mapper.Map<RegionDTO>(result);
            return Ok(regionDTO);
        }

        [HttpPost]
        [Route("saveRegion")]
        [ValidateModel]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create([FromBody] AddRegionDTO addRegionDTO)
        {
           
            // Convert DTO to domain model for database save
            var region = mapper.Map<Region>(addRegionDTO);
            region = await regionRepo.CreateAsync(region);

            // Map saved domain model back to DTO for client response
            var regionDTO = mapper.Map<RegionDTO>(region);

            return CreatedAtAction(nameof(GetByID), new { id = regionDTO.Id }, regionDTO);
           
        }

        [HttpPut]
        [Route("updateRegion/{Id:Guid}")]
        [ValidateModel]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Update([FromRoute] Guid Id, [FromBody] UpdateRegionDTO updateRegionDTO)
        {
           
            //map DTO to domain to send to repo
            var region = mapper.Map<Region>(updateRegionDTO);
            
            region = await regionRepo.UpdateAsync(Id, region);

            if(region is null)
            {
                return NotFound();
            }

            // Map updated domain model to DTO for client response
            var regionDTO = mapper.Map<RegionDTO>(region);
            
            return Ok(regionDTO);
            
        }

        [HttpDelete]
        [Route("deleteRegion/{Id:Guid}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete([FromRoute] Guid Id) 
        {
            var regionExists = await regionRepo.DeleteAsync(Id);

            if (regionExists is null)
            {
                return NotFound();
            }
            
            //map domain to DTO show client deleted item
            var regionDTO = mapper.Map<RegionDTO>(regionExists);

            return Ok(regionDTO);
        }
    }
}
