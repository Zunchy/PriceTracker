using System;
using System.Linq;
using System.Threading.Tasks;

namespace PriceTracker.Data
{
    public class getProducts
    {
        public Task<Product[]> GetProductAsync(DateTime startDate)
        {
            var rng = new Random();
            return Task.FromResult(Enumerable.Range(1, 5).Select(index => new Product
            {
                productName = "Example Product",
                Date = startDate.AddDays(index),
                currentPrice = rng.Next(1, 100),
                Review = rng.Next(1, 5)
            }).ToArray());
        }
    }
}
