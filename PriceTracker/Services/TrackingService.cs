using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PriceTracker.Data
{
    /// <summary>
    /// All product tracking logic is handled by the TrackingService
    /// </summary>
    public class TrackingService
    {
        private readonly IProductAccessLayer _product;
        private readonly IUserProductAccessLayer _userProduct;
        private readonly UserManager<ApplicationUser> _userManager;

        public TrackingService(IProductAccessLayer product, IUserProductAccessLayer userProduct, UserManager<ApplicationUser> userManager)
        {
            _product = product;
            _userProduct = userProduct;
            _userManager = userManager;
        }

        public async Task TrackItem(ClaimsPrincipal principle, DisplayItem item)
        {
            var user = await _userManager.GetUserAsync(principle);
            Product trackingProduct;

            string identifier;
            if(item.ItemSource == "Ebay")
            {
                identifier = item.ItemEbayId;
                trackingProduct = _product.GetProductByIdentifier(identifier);
            }
            else
            {
                identifier = item.ItemLink;
                trackingProduct = _product.GetProductByIdentifier(identifier);
            }

            if(trackingProduct == null)
            {
                trackingProduct = new Product
                {
                    Name = item.ItemName,
                    ProductIdentifier = identifier,
                    Source = item.ItemSource,
                    Users = new List<ApplicationUser>(),
                    PriceHistories = new List<ProductPriceHistory>()
                };
                trackingProduct.Users.Add(user);
                trackingProduct.PriceHistories.Add(new ProductPriceHistory 
                {
                    Timestamp = DateTime.UtcNow,
                    Price = (float)item.ItemPrice
                });

                await _product.AddProductAsync(trackingProduct);
            }
            else
            {
                trackingProduct.Users.Add(user);
            }
            /*dbContext.TrackedItems.Add(new TrackedItem
            {
                UserId = userManager.GetUserId(principle),
                //ItemIdentifier = itemIdentifier,
            });
            await dbContext.SaveChangesAsync();*/
        }

        public async Task UnTrackItem(ClaimsPrincipal principle, string ItemIdentifier)
        {
            //TrackedItem trackedItem = dbContext.TrackedItems.Find(ItemIdentifier);
            //dbContext.TrackedItems.Remove(trackedItem);
            //await dbContext.SaveChangesAsync();
        }

        public IQueryable<ApplicationUser> GetAllUsers()
        {
            return _userManager.Users;
        }

        public List<UserProduct> GetAllUserProducts()
        {
           return (List<UserProduct>)_userProduct.GetAllUserProducts();
        }

        public List<UserProduct> GetUserProductsByUserId(string userId)
        {
            return (List<UserProduct>)_userProduct.GetUserProductsByUserId(userId);
        }

        public Product GetProductById(int productId)
        {
            return _product.GetProduct(productId);
        }
    }
}
