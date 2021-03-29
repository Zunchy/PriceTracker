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
        private readonly EbayService _ebayService;

        public TrackingService(IProductAccessLayer product, IUserProductAccessLayer userProduct, UserManager<ApplicationUser> userManager, EbayService ebayService)
        {
            _product = product;
            _userProduct = userProduct;
            _userManager = userManager;
            _ebayService = ebayService;
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

        public async Task UpdateTrackedItemPrices()
        {
            //Get all products
            var productList = _product.GetAllProducts();
            //Loop through all products and retrieve current price from websites/api
            foreach(Product product in productList)
            {
                float currentPrice;
                if(product.Source == "Ebay")
                {
                    //Call Ebay Service, set currentPrice
                    var currentProduct = await _ebayService.GetProductAsync(product.ProductIdentifier);
                    currentPrice = (float)currentProduct.ItemPrice;
                }
                else
                {
                    //Call scraper, set current Price
                    currentPrice = 0; //Temp
                }

                //Get the newest recorded priceHistory
                var newestPriceHistory = product.PriceHistories.Aggregate((a, x) => x.Timestamp > a.Timestamp ? x : a);
                
                //If a change occurred create a new price history entry, else do nothing
                if (currentPrice != newestPriceHistory.Price)
                {
                    product.PriceHistories.Add(new ProductPriceHistory
                    {
                        Timestamp = DateTime.UtcNow,
                        Price = currentPrice
                    });
                    await _product.UpdateProductAsync(product);
                }
            }
        }

        //====================================
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
