using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WnT.API.CustomActionFilter;
using WnT.API.Models.Domain;
using WnT.API.Models.DTO.region;
using WnT.API.Models.DTO.walk;
using WnT.API.Repo.region;
using WnT.API.Repo.walk;

namespace WnT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IWalkRepo walkRepo;
        private readonly IMapper mapper;

        public WalksController(IWalkRepo walkRepo, IMapper mapper)
        {
            this.walkRepo = walkRepo;
            this.mapper = mapper;
        }

        //api/Walks/getall?filterOn='filteredColumn'&filterQuery='filteredWord'&sortBy='sortedColumn'&isAsc='ascOrDesc'&pageParam='pageNumber'&pageSize='PageSize'
        [HttpGet]
        [Route("getall")]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn,
                                                [FromQuery] string? filterQuery,
                                                [FromQuery] string? sortBy,
                                                [FromQuery] bool? isAsc,
                                                [FromQuery] int pageParam = 1,
                                                [FromQuery] int pageSize = 10)
        {
            var result = await walkRepo.GetAllAsync(filterOn, filterQuery, sortBy, isAsc ?? true, pageParam, pageSize);

            var walkDto = mapper.Map<List<WalkDTO>>(result);

            return Ok(walkDto);
        }

        [HttpGet]
        [Route("getWalkById/{Id:Guid}")]
        public async Task<IActionResult> GetByID([FromRoute] Guid Id)
        {
            var result = await walkRepo.GetByIdAsync(Id);

            if (result is null)
            {
                return NotFound();
            }

            var walkDTO = mapper.Map<WalkDTO>(result);

            return Ok(walkDTO);
        }

        [HttpPost]
        [Route("saveWalk")]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalkDTO addWalkDTO)
        {
            // Convert DTO to domain model for database save
            var walk = mapper.Map<Walk>(addWalkDTO);

            walk = await walkRepo.CreateAsync(walk);

            // Map saved domain model back to DTO for client response
            var walkDTO = mapper.Map<WalkDTO>(walk);

            return Ok(walkDTO);
        }

        [HttpPut]
        [Route("updateWalk/{Id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid Id, [FromBody] UpdateWalkDTO updateWalkDTO)
        {
            //map DTO to domain to send to repo
            var walk = mapper.Map<Walk>(updateWalkDTO);

            walk = await walkRepo.UpdateAsync(Id, walk);

            if (walk is null)
            {
                return NotFound();
            }

            // Map updated domain model to DTO for client response
            var walkDto = mapper.Map<WalkDTO>(walk);

            return Ok(walkDto);
        }

        [HttpDelete]
        [Route("deleteWalk/{Id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid Id)
        {
            var walkExists = await walkRepo.DeleteAsync(Id);

            if (walkExists is null)
            {
                return NotFound();
            }

            //map domain to DTO show client deleted item
            var walkDto = mapper.Map<WalkDTO>(walkExists);

            return Ok(walkDto);
        }
    }
}
