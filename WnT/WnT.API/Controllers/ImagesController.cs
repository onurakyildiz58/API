using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WnT.API.CustomActionFilter;
using WnT.API.Models.Domain;
using WnT.API.Models.DTO.image;
using WnT.API.Repo.image;

namespace WnT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepo imageRepo;
        private readonly IMapper mapper;

        public ImagesController(IImageRepo imageRepo, IMapper mapper)
        {
            this.imageRepo = imageRepo;
            this.mapper = mapper;
        }

        [HttpPost]
        [Route("saveImage")]
        [ValidateModel]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create([FromForm] AddImageDTO addImageDTO)
        {
            ValidateImage(addImageDTO);
            if(ModelState.IsValid)
            {
                var imageDomain = mapper.Map<Image>(addImageDTO);
                await imageRepo.CreateAsync(imageDomain);

                return Ok(imageDomain);
            }
           
            return BadRequest();
        }

        private void ValidateImage(AddImageDTO addImageDTO)
        {
            var allowedExtension = new string[] { ".jpg", ".jpeg", ".png" };

            if(!allowedExtension.Contains(Path.GetExtension(addImageDTO.File.FileName)))
            {
                ModelState.AddModelError("file", "unsupported file extension");
            }

            if(addImageDTO.File.Length > 10485760)
            {
                ModelState.AddModelError("file", "File size can't be more then 10MB");
            }
        }
    }
}
