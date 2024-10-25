using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WnT.API.Data;
using WnT.API.Models.Domain;
using WnT.API.Models.DTO;

namespace WnT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly WnTDbContext dbContext;

        public RegionsController(WnTDbContext dbContext)
        {
            this.dbContext = dbContext;
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
            // Retrieve all Regions as domain models from the database
            var result = await dbContext.Regions.ToListAsync();

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

            return Ok(regionDTO);
        }

        [HttpGet]
        [Route("getRegionById/{Id:Guid}")]
        public async Task<IActionResult> GetByID([FromRoute] Guid Id)
        {
            // Fetch a single Region as a domain model by ID
            var result = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == Id);

            if (result is null)
            {
                return NotFound();
            }

            // Map domain model to DTO for client response
            var regionDTO = new RegionDTO
            {
                Id = result.Id,
                Code = result.Code,
                Name = result.Name,
                RegionImageUrl = result.RegionImageUrl,
            };

            return Ok(regionDTO);
        }

        [HttpPost]
        [Route("saveRegion")]
        public async Task<IActionResult> Create([FromBody] AddRegionDTO addRegionDTO)
        {
            // Convert DTO to domain model for database save
            var region = new Region
            {
                Code = addRegionDTO.Code,
                Name = addRegionDTO.Name,
                RegionImageUrl = addRegionDTO.RegionImageUrl,
            };

            dbContext.Regions.Add(region);
            await dbContext.SaveChangesAsync();

            // Map saved domain model back to DTO for client response
            var regionDTO = new RegionDTO
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                RegionImageUrl = region.RegionImageUrl,
            };

            return CreatedAtAction(nameof(GetByID), new { id = regionDTO.Id }, regionDTO);
        }

        [HttpPut]
        [Route("updateRegion/{Id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid Id, [FromBody] UpdateRegionDTO updateRegionDTO)
        {
            // Find existing Region by ID
            var regionExists = await dbContext.Regions.FirstOrDefaultAsync(x=> x.Id == Id);

            if (regionExists is null)
            {
                return NotFound();
            }

            // Update domain model fields with data from DTO
            regionExists.Code = updateRegionDTO.Code;
            regionExists.Name = updateRegionDTO.Name;
            regionExists.RegionImageUrl = updateRegionDTO.RegionImageUrl;
            await dbContext.SaveChangesAsync();

            // Map updated domain model to DTO for client response
            var regionDTO = new RegionDTO
            {
                Id = regionExists.Id,
                Code = regionExists.Code,
                Name = regionExists.Name,
                RegionImageUrl = regionExists.RegionImageUrl,
            };

            return Ok(regionDTO);
        }

        [HttpDelete]
        [Route("deleteRegion/{Id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid Id) 
        {
            var regionExists = await dbContext.Regions.FirstOrDefaultAsync(x=>x.Id == Id);

            if(regionExists is null)
            {
                return NotFound();
            }

            dbContext.Regions.Remove(regionExists);
            await dbContext.SaveChangesAsync();

            //map domain to DTO show client deleted item
            var regionDTO = new RegionDTO
            {
                Id = regionExists.Id,
                Code = regionExists.Code,
                Name = regionExists.Name,
                RegionImageUrl = regionExists.RegionImageUrl,
            };

            return Ok(regionDTO);
        }
    }
}
