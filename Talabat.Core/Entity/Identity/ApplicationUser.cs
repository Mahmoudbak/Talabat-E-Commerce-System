using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entity.Identity
{
    public class ApplicationUser:IdentityUser
    {
        public string DispalyName { get; set; } = null!;
        public Address? Address { get; set; } = null!; //Navigational Property {One}

    }
}
