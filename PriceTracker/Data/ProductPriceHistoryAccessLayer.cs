using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PriceTracker.Data
{
    public interface IProductPriceHistoryAccessLayer
    {
        IEnumerable GetAllProductPriceHistories();
        Task AddProductPriceHistoryAsync(ProductPriceHistory ProductPriceHistory);
        Task UpdateProductPriceHistoryAsync(ProductPriceHistory ProductPriceHistory);
        ProductPriceHistory GetProductPriceHistory(int id);
        Task DeleteProductPriceHistoryAsync(int id);
    }

    public class ProductPriceHistoryAccessLayer : IProductPriceHistoryAccessLayer
    {
        private ApplicationDbContext _context;
        public ProductPriceHistoryAccessLayer(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable GetAllProductPriceHistories()
        {
            try
            {
                return _context.ProductPriceHistories.ToList();
            }
            catch
            {
                throw;
            }
        }

        public async Task AddProductPriceHistoryAsync(ProductPriceHistory ProductPriceHistory)
        {
            try
            {
                await _context.ProductPriceHistories.AddAsync(ProductPriceHistory);
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task UpdateProductPriceHistoryAsync(ProductPriceHistory ProductPriceHistory)
        {
            try
            {
                _context.Entry(ProductPriceHistory).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }

        public ProductPriceHistory GetProductPriceHistory(int id)
        {
            try
            {
                ProductPriceHistory productPriceHistory = _context.ProductPriceHistories.Find(id);
                return productPriceHistory;
            }
            catch
            {
                throw;
            }
        }

        public async Task DeleteProductPriceHistoryAsync(int id)
        {
            try
            {
                ProductPriceHistory productPriceHistory = _context.ProductPriceHistories.Find(id);
                _context.ProductPriceHistories.Remove(productPriceHistory);
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }
    }
}
