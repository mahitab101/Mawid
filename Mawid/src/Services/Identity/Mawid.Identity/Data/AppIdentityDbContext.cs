using Mawid.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Mawid.Identity.Data
{
    public class AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options)
       : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>(options);
}
