using System;

namespace PriceTracker.Data
{
    public class Product
    {
        public string productName { get; set; }
        public DateTime Date { get; set; }

        public decimal currentPrice  { get; set; }

        public decimal lowestPrice { get; set; }

        public double Review { get; set; }
    }
}
