using Microsoft.AspNetCore.Identity;

namespace Mawid.Identity.Models
{
    public class AppUser : IdentityUser<Guid>
    {
        public AppUser() => Id = Guid.CreateVersion7();
        public string FullName { get; set; } = string.Empty;
    }
}
