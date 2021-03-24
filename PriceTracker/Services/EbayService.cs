using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PriceTracker.Data
{
    /// <summary>
    /// Logic for retrieval of data from ebay's api
    /// </summary>
   
    public class EbayService
    {
        static HttpClient client;
        public IServiceProvider serviceProvider;
        private string currentToken;

        public EbayService(IServiceProvider services)
        {
            client = new HttpClient();
            serviceProvider = services;
            currentToken = "NoToken";
        }

        //Get a new Client Token
        public async Task<string> RequestClientTokenAsync()
        {
            //Clear Headers
            client.DefaultRequestHeaders.Clear();

            //Setting Up Request
            string LoginURL = "https://api.ebay.com/identity/v1/oauth2/token";
            string LoginInfo = "grant_type=client_credentials&scope=https%3A%2F%2Fapi.ebay.com%2Foauth%2Fapi_scope";
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpContent payload = new StringContent(LoginInfo, Encoding.UTF8, "application/x-www-form-urlencoded");

            //Client-Id:Client-Secret Must be encoded Base64
            var plainTextBytesCredentials = System.Text.Encoding.UTF8.GetBytes("ZacharyW-Capstone-PRD-ddc6cc39b-adca02d8:PRD-dc6cc39b27ee-a3fc-4917-b64f-2d8f");
            var b64Credentials = System.Convert.ToBase64String(plainTextBytesCredentials);
            client.DefaultRequestHeaders.Add("Authorization", "Basic " + b64Credentials);

            //Getting a response
            HttpResponseMessage response = await client.PostAsync(LoginURL, payload);
            string content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                //Save response as the current token and return display success msg
                var accessTokenResponse = JsonConvert.DeserializeObject<AccessTokenResponse>(content);
                currentToken = accessTokenResponse.access_token;
                return "Successfully retrieved client token";
            }
            else
            {
                return content;
            }
        }

        //Search Ebay Api 
        public  async Task<List<DisplayItem>> SearchProductAsync(string query, int numResults)
        {
            //Clear Headers
            client.DefaultRequestHeaders.Clear();

            //Set Authorization header to contain the current token
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + currentToken);

            string content = null;
            HttpResponseMessage response = await client.GetAsync($"https://api.ebay.com/buy/browse/v1/item_summary/search?q={query}&limit={numResults}");
            content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                //List of DisplayItems To Return
                List<DisplayItem> displayItems = new List<DisplayItem>();

                //If Successfull deserialize content into object
                var ebaySearch = JsonConvert.DeserializeObject<EbaySearch>(content);

                //Return list of search items
                foreach(var product in ebaySearch.itemSummaries)
                {
                    DisplayItem item = new DisplayItem();
                    item.ItemName = product.title;
                    item.ItemPrice = Convert.ToDouble(product.price.value);
                    item.ItemLink = product.itemWebUrl;
                    item.ItemImage = product.image.imageUrl;
                    item.ItemSource = "Ebay";
                    displayItems.Add(item);
                }

                return displayItems;
            }
            else
            {
                var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(content);
                if(errorResponse.errors.First().message == "Invalid access token")
                {
                    //Attempt to refresh token
                    await RequestClientTokenAsync();
                    return await SearchProductAsync(query, numResults);
                }
                else
                {
                    //return error if token refresh fails
                    return null;
                }
            }
        }
    }

    //Classes to represent json objects returned by Ebay Api Calls
    public class AccessTokenResponse
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public string token_type { get; set; }
    }

    public class Parameter
    {
        public string name { get; set; }
        public string value { get; set; }
    }

    public class Error
    {
        public int errorId { get; set; }
        public string domain { get; set; }
        public string category { get; set; }
        public string message { get; set; }
        public List<string> inputRefIds { get; set; }
        public List<Parameter> parameters { get; set; }
    }

    public class ErrorResponse
    {
        public List<Error> errors { get; set; }
    }


    public class EbayImage
    {
        public string imageUrl { get; set; }
    }

    public class EbayPrice
    {
        public string value { get; set; }
        public string currency { get; set; }
    }

    public class EbaySeller
    {
        public string username { get; set; }
        public string feedbackPercentage { get; set; }
        public int feedbackScore { get; set; }
    }

    public class EbayThumbnailImage
    {
        public string imageUrl { get; set; }
    }

    public class EbayShippingCost
    {
        public string value { get; set; }
        public string currency { get; set; }
    }

    public class EbayShippingOption
    {
        public string shippingCostType { get; set; }
        public EbayShippingCost shippingCost { get; set; }
    }

    public class EbayItemLocation
    {
        public string postalCode { get; set; }
        public string country { get; set; }
    }

    public class EbayCategory
    {
        public string categoryId { get; set; }
    }

    public class EbayAdditionalImage
    {
        public string imageUrl { get; set; }
    }

    public class EbayItemSummary
    {
        public string itemId { get; set; }
        public string title { get; set; }
        public EbayImage image { get; set; }
        public EbayPrice price { get; set; }
        public string itemHref { get; set; }
        public EbaySeller seller { get; set; }
        public string condition { get; set; }
        public string conditionId { get; set; }
        public List<EbayThumbnailImage> thumbnailImages { get; set; }
        public List<EbayShippingOption> shippingOptions { get; set; }
        public List<string> buyingOptions { get; set; }
        public string itemWebUrl { get; set; }
        public EbayItemLocation itemLocation { get; set; }
        public List<EbayCategory> categories { get; set; }
        public List<EbayAdditionalImage> additionalImages { get; set; }
        public bool adultOnly { get; set; }
        public string legacyItemId { get; set; }
        public bool availableCoupons { get; set; }
        public string epid { get; set; }
    }

    public class EbaySearch
    {
        public string href { get; set; }
        public int total { get; set; }
        public string next { get; set; }
        public int limit { get; set; }
        public int offset { get; set; }
        public List<EbayItemSummary> itemSummaries { get; set; }
    }

}
