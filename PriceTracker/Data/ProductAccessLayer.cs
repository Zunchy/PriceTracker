using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PriceTracker.Data
{
    public interface IProductsAccessLayer
    {
        IEnumerable GetAllProducts();
        Task AddProductsAsync(Product Products);
        Task UpdateProductsAsync(Product Products);
        Product GetProducts(int id);
        Product GetProductsByIdentifier(string ProductsIdentifier);
        Task DeleteProductsAsync(int id);
    }
    
    public class ProductsAccessLayer : IProductsAccessLayer
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
                return _context.Products.ToList();
            }
            catch
            {
                throw;
            }
        }

        public async Task AddProductsAsync(Product Products)
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

        public async Task UpdateProductsAsync(Product Products)
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

        public Product GetProducts(int id)
        {
            try
            {
                Product Products = _context.Products.Find(id);
                return Products;
            }
            catch
            {
                throw;
            }
        }

        public Product GetProductsByIdentifier(string ProductsIdentifier)
        {
            try
            {
                Product Products = _context.Products
                                          .Where(p => p.ProductIdentifier == ProductsIdentifier)
                                          .FirstOrDefault();
                return Products;
            }
            catch(Exception e)
            {
                throw;
            }
        }

        public async Task DeleteProductsAsync(int id)
        {
            try
            {
                Product Products = _context.Products.Find(id);
                _context.Products.Remove(Products);
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }
    }
}
