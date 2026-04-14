using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entity.Identity;

namespace Talabat.Repsotiory._Identity
{
    public static class ApplicationIdentitycontextSeed
    {
        public static async Task SeedUserAsync(UserManager<ApplicationUser> userManger)
        {
            if (!userManger.Users.Any())
            {
                var user = new ApplicationUser()
                {
                    DispalyName="Mahmoud Bakr",
                    Email="MahmoudBakr@gmail.com"
                    ,UserName="Mahmoud.Bakr",
                    PhoneNumber="01273318932"
                };
                await userManger.CreateAsync(user, "P@ssw0rd");
            }
        }

    }
}
