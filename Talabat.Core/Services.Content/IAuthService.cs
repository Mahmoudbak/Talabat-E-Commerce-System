using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entity.Identity;

namespace Talabat.Core.Services.Content
{
    public interface IAuthService
    {
        Task<string> CreateTokenAsync(ApplicationUser user,UserManager<ApplicationUser>userManager);
    }
}
