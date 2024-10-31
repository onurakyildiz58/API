using Microsoft.AspNetCore.Identity;

namespace WnT.API.Repo.token
{
    public interface ITokenRepo
    {
        string GenerateJWTToken(IdentityUser user, List<string> roles);
    }
}
