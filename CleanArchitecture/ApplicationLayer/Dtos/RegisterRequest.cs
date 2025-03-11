
namespace ApplicationLayer.Dtos
{
    public class RegisterRequest
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string PasswordHash { get; set; }

        public string Role { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }

        public string Email { get; set; }
    }
}
