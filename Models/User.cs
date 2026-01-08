using Microsoft.AspNetCore.Identity;

namespace EnglishCenter.Models
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Avatar { get; set; }
        public string? Address { get; set; }
        public bool? IsDeleted { get; set; } = false;
    }
}
