
using System.ComponentModel.DataAnnotations;

namespace DomainLayer.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public string Role { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }



    }
}
