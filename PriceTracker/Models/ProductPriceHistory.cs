using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PriceTracker.Data
{
    public class ProductPriceHistory
    {
        [Key]
        public int PriceHistoryId { get; set; }
        public byte[] Timestamp { get; set; }
        public float Price { get; set; }
    }
}
