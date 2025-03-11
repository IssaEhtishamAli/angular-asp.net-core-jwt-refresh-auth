

using ApplicationLayer.Dtos;
using static ApplicationLayer.Generic.mGeneric;

namespace ApplicationLayer.Repositriy
{
    public interface IAuthRepositriy
    {
        Task<mApiResponse<string>> RegisterUser(RegisterRequest usr);
        Task<mApiResponse<string>> AuthenticateUser(string email, string password);
    }
}
