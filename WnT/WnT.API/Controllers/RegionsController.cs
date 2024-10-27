using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using WnT.API.Data;
using WnT.API.Models.Domain;
using WnT.API.Models.DTO;
using WnT.API.Repo;

namespace WnT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly IRegionRepo regionRepo;
        private readonly IMapper mapper;

        public RegionsController( IRegionRepo regionRepo, IMapper mapper)
        {
            this.regionRepo = regionRepo;
            this.mapper = mapper;
        }

        /*
          DTOs: Used for client responses to define the data shape needed by the client.
          Domain Models: Used for database entities and data fetching/saving operations.
          - Fetching: Retrieve data as domain models from the database, then map to DTOs for client responses.
          - Saving: Convert DTOs to domain models before saving to the database; map saved data back to DTOs if needed.
        */

        [HttpGet]
        [Route("getall")]
        public async Task<IActionResult> GetAll()
        {
            /*
            BEFORE REPOSITORY PATTERN

            // Retrieve all Regions as domain models from the database
            var result = await dbContext.Regions.ToListAsync();
            */

            var result = await regionRepo.GetAllAsync();

            /*   
            BEFORE AUTOMAPPER
            
            // Map each domain model to a DTO for client response
            var regionDTO = new List<RegionDTO>();
            foreach (var region in result) 
            {
                regionDTO.Add(new RegionDTO()
                {
                    Id = region.Id,
                    Code = region.Code,
                    Name = region.Name,
                    RegionImageUrl = region.RegionImageUrl,
                });
            }    
            */

            // Map each domain model to a DTO for client response
            var regionDTO = mapper.Map<List<RegionDTO>>(result);

            return Ok(regionDTO);
        }

        [HttpGet]
        [Route("getRegionById/{Id:Guid}")]
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
