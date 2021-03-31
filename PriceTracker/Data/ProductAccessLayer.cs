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
        Task AddProductAsync(Product Products);
        Task UpdateProductAsync(Product Products);
        Product GetProduct(int id);
        Product GetProductByIdentifier(string ProductsIdentifier);
        Task DeleteProductAsync(int id);
    }
    
    public class ProductsAccessLayer : IProductAccessLayer
    {
        private ApplicationDbContext _context;
        public ProductsAccessLayer(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable GetAllProducts()
        {
            try
            {
                return _context.Products
                    .Include(p => p.PriceHistories)
                    .ToList();
            }
            catch
            {
                throw;
            }
        }

        public async Task AddProductAsync(Product Products)
        {
            try
            {
                await _context.Products.AddAsync(Products);
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task UpdateProductAsync(Product Products)
        {
            try
            {
                _context.Entry(Products).State = EntityState.Modified;
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
                Product product = _context.Products.Include(p => p.PriceHistories)
                                                    .Where(p => p.ProductId == id)
                                                    .FirstOrDefault();
                return product;
            }
            catch
            {
                throw;
            }
        }

        public Product GetProductByIdentifier(string ProductsIdentifier)
        {
            try
            {
                Product products = _context.Products
                                          .Include(p => p.PriceHistories)
                                          .Include(p => p.Users)
                                          .Where(p => p.ProductIdentifier == ProductsIdentifier)
                                          .FirstOrDefault();
                return products;
            }
            catch(Exception e)
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
