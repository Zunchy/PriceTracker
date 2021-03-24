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

        private ChromeOptions options;
        private IWebDriver mercariDriver, eBidDriver, poshmarkDriver, eCraterDriver;

        public Scraper()
        {
            options = new ChromeOptions();
            options.AddArguments("--headless");
        }

        public String ScrapeMercari(string searchTerm)
        {
            mercariDriver = new ChromeDriver(options);

            // Search For Products

            String searchUrl = $"https://www.mercari.com/search/?keyword={searchTerm}";
            mercariDriver.Navigate().GoToUrl(searchUrl);

            IReadOnlyCollection<IWebElement> searchSpace = mercariDriver.FindElements(By.XPath("//div[@data-testid=\"ItemContainer\"]"));

            String result = "";
            foreach (IWebElement item in searchSpace)
            {
                try
                {
                    IWebElement itemContent = item.FindElement(By.XPath("./.."));
                    String link = itemContent.GetAttribute("href");
                    String image = "";

                    result += itemContent.Text;
                    result += "\r\n";
                    result += link;
                    result += "\r\n";

                    String imagePath = link.Split("/")[5];
                    image = $"https://mercari-images.global.ssl.fastly.net/photos/{imagePath}_1.jpg?1615326085&w=200&h=200&fitcrop&sharpen";

                    result += image;
                    result += "END ITEM";
                }
                catch { }

            }

            mercariDriver.Quit();

            return result;

        }

        public double ScrapePriceByMercariItem(string itemLink)
        {
            mercariDriver = new ChromeDriver(options);

            double newPrice = 0.0;

            mercariDriver.Navigate().GoToUrl(itemLink);
            String sitePrice = mercariDriver.FindElement(By.XPath("//*[contains(@class, 'ProductPrice')]")).Text;

            newPrice = Double.Parse(sitePrice.Substring(1, sitePrice.Length - 1));

            mercariDriver.Quit();

            return newPrice;
        }

        public String ScrapeEBid(string searchTerm)
        {
            eBidDriver = new ChromeDriver(options);

            // Search For Products

            String searchUrl = $"https://www.ebid.net/us/perl/main.cgi?mo=search&words={searchTerm}";
            eBidDriver.Navigate().GoToUrl(searchUrl);

            IReadOnlyCollection<IWebElement> searchSpace = eBidDriver.FindElements(By.XPath("//li[@class='showroomcell']"));

            String result = "";
            foreach (IWebElement item in searchSpace)
            {
                try
                {
                    String name = item.FindElement(By.TagName("h2")).Text;
                    String price = item.FindElement(By.ClassName("dkgrey")).Text;
                    String link = item.FindElement(By.TagName("a")).GetAttribute("href");
                    String image = item.FindElement(By.TagName("img")).GetAttribute("src");

                    result += name;
                    result += "\r\n";
                    result += price;
                    result += "\r\n";
                    result += link;
                    result += "\r\n";
                    result += image;
                    result += "END ITEM";
                }
                catch { }
            }

            eBidDriver.Quit();

            return result;
        }

        public double ScrapePriceByEbidItem(string itemLink)
        {
            eBidDriver = new ChromeDriver(options);

            double newPrice = 0.0;

            eBidDriver.Navigate().GoToUrl(itemLink);
            String sitePrice = eBidDriver.FindElement(By.XPath("//*[contains(@class, 'exchangedPrice')]")).Text;

            newPrice = Double.Parse(sitePrice.Substring(2, sitePrice.Length - 3));

            eBidDriver.Quit();

            return newPrice;
        }

        public String ScrapePoshmark(string searchTerm)
        {
            poshmarkDriver = new ChromeDriver(options);

            // Search For Products
            String searchUrl = $"https://poshmark.com/search?query={searchTerm}";
            poshmarkDriver.Navigate().GoToUrl(searchUrl);

            ((IJavaScriptExecutor)poshmarkDriver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");

            IReadOnlyCollection<IWebElement> searchSpace = poshmarkDriver.FindElements(By.XPath("//*[contains(@class, 'card')]"));

            String result = "";
            foreach (IWebElement item in searchSpace)
            {
                try
                {
                    String link = item.FindElement(By.TagName("a")).GetAttribute("href");
                    String image = item.FindElement(By.TagName("img")).GetAttribute("src");

                    result += item.Text;
                    result += "\r\n";
                    result += link;
                    result += "\r\n";
                    result += image;
                    result += "END ITEM";
                }
                catch { }
            }

            poshmarkDriver.Quit();

            return result;
        }

        public double ScrapePriceByPoshmarkItem(string itemLink)
        {
            poshmarkDriver = new ChromeDriver(options);

            double newPrice = 0.0;

            poshmarkDriver.Navigate().GoToUrl(itemLink);
            String siteInfo = poshmarkDriver.FindElement(By.XPath("//*[contains(@class, 'listing__info')]")).Text;

            newPrice = Double.Parse(siteInfo.Split('$')[1].Replace(" ", String.Empty));

            poshmarkDriver.Quit();

            return newPrice;
        }

        public String ScrapeEcrater(string searchTerm)
        {
            eCraterDriver = new ChromeDriver(options);

            // Search For Products
            String searchUrl = $"https://www.ecrater.com/filter.php?keywords={searchTerm}";
            eCraterDriver.Navigate().GoToUrl(searchUrl);

            IReadOnlyCollection<IWebElement> searchSpace = eCraterDriver.FindElements(By.XPath("//*[contains(@class, 'product-item')]"));

            String result = "";
            foreach (IWebElement item in searchSpace)
            {
                try
                {
                    String link = item.FindElement(By.TagName("a")).GetAttribute("href");
                    String image = item.FindElement(By.TagName("img")).GetAttribute("src");

                    result += item.Text;
                    result += "\r\n";
                    result += link;
                    result += "\r\n";
                    result += image;
                    result += "END ITEM";
                }
                catch { }
            }

            eCraterDriver.Quit();

            return result;
        }

        public double ScrapePriceByEcraterItem(string itemLink)
        {
            eCraterDriver = new ChromeDriver(options);

            double newPrice = 0.0;

            eCraterDriver.Navigate().GoToUrl(itemLink);
            String siteInfo = eCraterDriver.FindElement(By.XPath("//*[contains(@id, 'product-title-actions')]")).Text;

            String[] priceInfo = siteInfo.Split(" ");

            newPrice = Double.Parse(priceInfo[0].Substring(1, priceInfo[0].Length - 1));

            eCraterDriver.Quit();

            return newPrice;
        }
    }
}
