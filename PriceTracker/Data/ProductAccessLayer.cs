using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PriceTracker.Data
{
    public interface IProductAccessLayer
    {
        IEnumerable GetAllProducts();
        Task AddProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Product GetProduct(int id);
        Product GetProductByIdentifier(string productIdentifier);
        Task DeleteProductAsync(int id);
    }
    
    public class ProductAccessLayer : IProductAccessLayer
    {
        private ApplicationDbContext _context;
        public ProductAccessLayer(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable GetAllProducts()
        {
            try
            {
                return _context.Products.ToList();
            }
            catch
            {
                throw;
            }
        }

        public async Task AddProductAsync(Product product)
        {
            try
            {
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task UpdateProductAsync(Product product)
        {
            try
            {
                _context.Entry(product).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }

        public Product GetProduct(int id)
        {
            try
            {
                Product product = _context.Products.Find(id);
                return product;
            }
            catch
            {
                throw;
            }
        }

        public Product GetProductByIdentifier(string productIdentifier)
        {
            try
            {
                Product product = _context.Products
                                          .Where(p => p.ProductIdentifier == productIdentifier)
                                          .FirstOrDefault();
                return product;
            }
            catch
            {
                throw;
            }
        }

        public async Task DeleteProductAsync(int id)
        {
            try
            {
                Product product = _context.Products.Find(id);
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }
    }
}
