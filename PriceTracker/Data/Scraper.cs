using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.IO;
using System.Net.Http;
using System.Text;

namespace PriceTracker.Data
{
    public class Scraper
    {

        ChromeOptions options;
        IWebDriver driver;

        public Scraper()
        {
            options = new ChromeOptions();
            options.AddArguments("--headless");

            driver = new ChromeDriver(options);
        }

        public String ScrapeMercari(string searchTerm)
        {

            // Search For Products

            String searchUrl = $"https://www.mercari.com/search/?keyword={searchTerm}";
            driver.Navigate().GoToUrl(searchUrl);
            //IReadOnlyCollection<IWebElement> test = driver.FindElements(By.TagName("li"));
            IReadOnlyCollection<IWebElement> searchSpace = driver.FindElements(By.XPath("//div[@data-testid=\"ItemContainer\"]"));

            String result = "";
            foreach (IWebElement item in searchSpace)
            {
                IWebElement itemContent = item.FindElement(By.XPath("./.."));
                String link = itemContent.GetAttribute("href");

                result += itemContent.Text;
                result += "\r\n";
                result += link;
                result += "END ITEM";

            }

            return result;

        }

        public double ScrapePriceByMercariItem(string itemLink)
        {
            double newPrice = 0.0;

            driver.Navigate().GoToUrl(itemLink);
            String sitePrice = driver.FindElement(By.XPath("//*[contains(@class, 'ProductPrice')]")).Text;

            newPrice = Double.Parse(sitePrice.Substring(1, sitePrice.Length - 1));

            return newPrice;
        }

        public String ScrapeEBid(string searchTerm)
        {
            // Search For Products

            String searchUrl = $"https://www.ebid.net/us/perl/main.cgi?mo=search&words={searchTerm}";
            driver.Navigate().GoToUrl(searchUrl);

            //IReadOnlyCollection<IWebElement> test = driver.FindElements(By.TagName("li"));
            IReadOnlyCollection<IWebElement> searchSpace = driver.FindElements(By.XPath("//li[@class='showroomcell']"));


            String result = "";
            foreach (IWebElement item in searchSpace)
            {

                String name = item.FindElement(By.TagName("h2")).Text;
                String price = item.FindElement(By.ClassName("dkgrey")).Text;
                String link = item.FindElement(By.TagName("a")).GetAttribute("href");

                result += name;
                result += "\r\n";
                result += price;
                result += "\r\n";
                result += link;
                result += "END ITEM";

            }

            return result;
        }

        public double ScrapePriceByEbidItem(string itemLink)
        {
            double newPrice = 0.0;

            driver.Navigate().GoToUrl(itemLink);
            String sitePrice = driver.FindElement(By.XPath("//*[contains(@class, 'exchangedPrice')]")).Text;

            newPrice = Double.Parse(sitePrice.Substring(2, sitePrice.Length - 3));

            return newPrice;
        }

        public String ScrapePoshmark(string searchTerm)
        {
            // Search For Products
            String searchUrl = $"https://poshmark.com/search?query={searchTerm}";
            driver.Navigate().GoToUrl(searchUrl);

            //IReadOnlyCollection<IWebElement> test = driver.FindElements(By.TagName("li"));
            IReadOnlyCollection<IWebElement> searchSpace = driver.FindElements(By.XPath("//*[contains(@class, 'card')]"));


            String result = "";
            foreach (IWebElement item in searchSpace)
            {

                String link = item.FindElement(By.TagName("a")).GetAttribute("href");

                result += item.Text;
                result += "\r\n";
                result += link;
                result += "END ITEM";

            }

            return result;
        }

        public double ScrapePriceByPoshmarkItem(string itemLink)
        {
            double newPrice = 0.0;

            driver.Navigate().GoToUrl(itemLink);
            String siteInfo = driver.FindElement(By.XPath("//*[contains(@class, 'listing__info')]")).Text;

            newPrice = Double.Parse(siteInfo.Split('$')[1].Replace(" ", String.Empty));

            return newPrice;
        }

        public String ScrapeEcrater(string searchTerm)
        {
            // Search For Products
            String searchUrl = $"https://www.ecrater.com/filter.php?keywords={searchTerm}";
            driver.Navigate().GoToUrl(searchUrl);

            //IReadOnlyCollection<IWebElement> test = driver.FindElements(By.TagName("li"));
            IReadOnlyCollection<IWebElement> searchSpace = driver.FindElements(By.XPath("//*[contains(@class, 'product-item')]"));


            String result = "";
            foreach (IWebElement item in searchSpace)
            {

                String link = item.FindElement(By.TagName("a")).GetAttribute("href");

                result += item.Text;
                result += "\r\n";
                result += link;
                result += "END ITEM";

            }

            return result;
        }

        public double ScrapePriceByEcraterItem(string itemLink)
        {
            double newPrice = 0.0;

            driver.Navigate().GoToUrl(itemLink);
            String siteInfo = driver.FindElement(By.XPath("//*[contains(@id, 'product-title-actions')]")).Text;

            String[] priceInfo = siteInfo.Split(" ");

            newPrice = Double.Parse(priceInfo[0].Substring(1, priceInfo[0].Length - 1));

            return newPrice;
        }
    }
}
