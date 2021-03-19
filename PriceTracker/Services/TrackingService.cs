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
        private readonly UserManager<ApplicationUser> _userManager;

        public TrackingService(IProductAccessLayer product, UserManager<ApplicationUser> userManager)
        {
            _product = product;
            _userManager = userManager;
        }

        public async Task TrackItem(ClaimsPrincipal principle, string productName, string productIdentifier)
        {
            var user = await _userManager.GetUserAsync(principle);
            var trackingProduct = _product.GetProductByIdentifier(productIdentifier);
            if(trackingProduct == null)
            {
                trackingProduct = new Product
                {
                    Name = productName,
                    ProductIdentifier = productIdentifier,
                    Source = "Test"
                };
                trackingProduct.Users.Add(user);

                await _product.AddProductAsync(trackingProduct);
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
    }
}
