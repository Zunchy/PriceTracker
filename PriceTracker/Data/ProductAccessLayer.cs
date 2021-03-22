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
        IEnumerable GetAllProductss();
        Task AddProductsAsync(Products Products);
        Task UpdateProductsAsync(Products Products);
        Products GetProducts(int id);
        Products GetProductsByIdentifier(string ProductsIdentifier);
        Task DeleteProductsAsync(int id);
    }
    
    public class ProductsAccessLayer : IProductsAccessLayer
    {
        private ApplicationDbContext _context;
        public ProductsAccessLayer(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable GetAllProductss()
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

        public async Task AddProductsAsync(Products Products)
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

        public async Task UpdateProductsAsync(Products Products)
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

        public Products GetProducts(int id)
        {
            try
            {
                Products Products = _context.Products.Find(id);
                return Products;
            }
            catch
            {
                throw;
            }
        }

        public Products GetProductsByIdentifier(string ProductsIdentifier)
        {
            try
            {
                Products Products = _context.Products
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
                Products Products = _context.Products.Find(id);
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
