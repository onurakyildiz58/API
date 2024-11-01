using WnT.API.Models.Domain;

namespace WnT.API.Repo.image
{
    public interface IImageRepo
    {
        Task<Image> CreateAsync(Image image);
    }
}                                                                                                       
