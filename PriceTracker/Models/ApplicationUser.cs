using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PriceTracker.Data
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Product> Products { get; set; }
        public List<UserProduct> UserProducts { get; set; }
    }
}
