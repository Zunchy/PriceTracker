using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
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
        private readonly IEmailSender _emailSender;

        public TrackingService(IProductAccessLayer product, IUserProductAccessLayer userProduct, UserManager<ApplicationUser> userManager, EbayService ebayService, IEmailSender emailSender)
        {
            _product = product;
            _userProduct = userProduct;
            _userManager = userManager;
            _ebayService = ebayService;
            _emailSender = emailSender;
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
                if (!trackingProduct.Users.Contains(user))
                {
                    trackingProduct.Users.Add(user);
                }
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

            Scraper scraper = new Scraper();
            IQueryable<ApplicationUser> users = GetAllUsers();

            List<UserProduct> productstoBeUpdated = new List<UserProduct>();

            foreach (ApplicationUser user in users)
            {
                //Get all products for the user
                var productList = _userProduct.GetUserProductsByUserId(user.Id);

                String emailMessage = String.Empty;
                int itemChangeCount = 0;

                //Loop through all products and retrieve current price from websites/api
                foreach (UserProduct userProduct in productList)
                {
                    float currentPrice = 0.0F;
                    if (userProduct.Product.Source == "Ebay")
                    {
                        //Call Ebay Service, set currentPrice
                        var currentProduct = await _ebayService.GetProductAsync(userProduct.Product.ProductIdentifier);
                        currentPrice = (float)currentProduct.ItemPrice;
                    }
                    else
                    {
                        switch (userProduct.Product.Source)
                        {
                            case "Mercari":
                                currentPrice = (float)scraper.ScrapePriceByMercariItem(userProduct.Product.ProductIdentifier);
                                break;
                            case "eBid":
                                currentPrice = (float)scraper.ScrapePriceByEbidItem(userProduct.Product.ProductIdentifier);
                                break;
                            case "Poshmark":
                                currentPrice = (float)scraper.ScrapePriceByPoshmarkItem(userProduct.Product.ProductIdentifier);
                                break;
                            case "eCrater":
                                currentPrice = (float)scraper.ScrapePriceByEcraterItem(userProduct.Product.ProductIdentifier);
                                break;
                        }
                    }

                    //Get the newest recorded priceHistory
                    var newestPriceHistory = userProduct.Product.PriceHistories.Aggregate((a, x) => x.Timestamp > a.Timestamp ? x : a);


                    //If a change occurred create a new price history entry, else do nothing
                    if (currentPrice != newestPriceHistory.Price)
                    {

                        ++itemChangeCount;

                        emailMessage += "The Price of " + userProduct.Product.Name + " has changed from $" + newestPriceHistory.Price + " to $" + currentPrice + "<br>"; 

                        userProduct.Product.PriceHistories.Add(new ProductPriceHistory
                        {
                            Timestamp = DateTime.UtcNow,
                            Price = currentPrice
                        });
                        productstoBeUpdated.Add(userProduct);
                        
                    }
                }

                if(itemChangeCount > 0)
                    await _emailSender.SendEmailAsync(user.Email, "eTrack Item Price changes for " + itemChangeCount + " Items", emailMessage);

            }

            foreach (UserProduct productToBeUpdated in productstoBeUpdated)
                await _product.UpdateProductAsync(productToBeUpdated.Product);
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

        public ApplicationUser GetUserByPrincipal(ClaimsPrincipal principal)
        {
            return _userManager.Users.Where(u => u.Email == principal.Identity.Name).First();
        }
    }
}
