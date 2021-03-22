using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PriceTracker.Data
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string ProductIdentifier { get; set; }
        public string Source { get; set; }

        public ICollection<ApplicationUser> Users { get; set; }
        public List<UserProduct> UserProducts { get; set; }
    }
}
