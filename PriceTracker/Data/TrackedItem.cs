using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PriceTracker.Data
{
    public class TrackedItem
    {
        [Key]
        public string UserId { get; set; }
        public string ItemIdentifier { get; set; }
    }
}
