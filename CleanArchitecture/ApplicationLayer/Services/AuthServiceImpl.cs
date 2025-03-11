

using ApplicationLayer.Dtos;
using ApplicationLayer.Repositriy;
using static ApplicationLayer.Generic.mGeneric;


namespace ApplicationLayer.Services
{
    public class AuthServiceImpl : IAuthService
    {
        private readonly IAuthRepositriy _authRepositriy;
        public AuthServiceImpl(IAuthRepositriy authRepositriy)
        {
            _authRepositriy = authRepositriy;
        }
        public Task<mApiResponse<string>> AuthenticateUser(string email, string password)
        {
            return _authRepositriy.AuthenticateUser(email, password);
        }

        public Task<mApiResponse<string>> RegisterUser(RegisterRequest usrBody)
        {
            return _authRepositriy.RegisterUser(usrBody);
        }
    }
}
