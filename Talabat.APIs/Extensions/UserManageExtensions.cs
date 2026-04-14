using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Talabat.Core.Entity.Identity;

namespace Talabat.APIs.Extensions
{
    public static class UserManageExtensions
    {
        public static async Task<ApplicationUser?> FindUserWithAddressAsync(this UserManager<ApplicationUser> userManager, ClaimsPrincipal User)
        {
            var email = User.FindFirstValue(ClaimTypes.Email) ;

            var user = await userManager.Users.Include(U=>U.Address).SingleOrDefaultAsync(U=>U.Email==email);

            return user;    
        }

    }
}
