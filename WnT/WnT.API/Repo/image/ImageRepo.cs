using Microsoft.EntityFrameworkCore;
using WnT.API.Data;
using WnT.API.Models.Domain;

namespace WnT.API.Repo.image
{
    public class ImageRepo : IImageRepo
    {
        private readonly WnTDbContext dbContext;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ImageRepo(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, WnTDbContext dbContext)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor; 
            this.dbContext = dbContext;
        }

        public async Task<Image> CreateAsync(Image image)
        {
            var localFilePath = Path.Combine(webHostEnvironment.ContentRootPath, "Images", $"{image.FileName}{image.FileExtension}");

            // upload to local path
            using var stream = new FileStream(localFilePath, FileMode.Create);
            await image.File.CopyToAsync(stream);

            // https://localhost:1234/images/image.jpeg
            var urlFilePath = $"{httpContextAccessor.HttpContext.Request.Scheme}://" +
                                $"{httpContextAccessor.HttpContext.Request.Host}{httpContextAccessor.HttpContext.Request.PathBase}" +
                                $"/Images/{image.FileName}{image.FileExtension}";

            image.FilePath = urlFilePath;

            await dbContext.Images.AddAsync(image);
            await dbContext.SaveChangesAsync();
            return image;
        }
    }
}
