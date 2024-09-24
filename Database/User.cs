using Microsoft.AspNetCore.Identity;

namespace PTManagementSystem.Database
{
    public class User : IdentityUser
    {
        public string? Initials {  get; set; }
    }
}
