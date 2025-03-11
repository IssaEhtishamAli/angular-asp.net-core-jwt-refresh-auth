

using ApplicationLayer.Dtos;
using static ApplicationLayer.Generic.mGeneric;

namespace ApplicationLayer.Services
{
    public interface IAuthService
    {
        Task<mApiResponse<string>> RegisterUser(RegisterRequest usrBody);
        Task<mApiResponse<string>> AuthenticateUser(string email, string password);
    }
}
