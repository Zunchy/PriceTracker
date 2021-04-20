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

        public String ScrapeMercari(string searchTerm, bool shouldBeSearched)
        {
            if (shouldBeSearched)
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
            else
            {
                return "";
            }
        }

        public string ScrapeImageByMercariItem(string itemLink)
        {
            try
            {
                mercariDriver = new ChromeDriver(options);

                string image = "";

                String imagePath = itemLink.Split("/")[5];
                image = $"https://mercari-images.global.ssl.fastly.net/photos/{imagePath}_1.jpg?1615326085&w=200&h=200&fitcrop&sharpen";

                mercariDriver.Quit();

                return image;
            }
            catch { return ""; }
        }

        public double ScrapePriceByMercariItem(string itemLink)
        {
            mercariDriver = new ChromeDriver(options);

            double newPrice = 0.0;

            mercariDriver.Navigate().GoToUrl(itemLink);

            String sitePrice = mercariDriver.FindElement(By.XPath("//meta[@property=\"product:price:amount\"]")).GetAttribute("content");

            newPrice = Double.Parse(sitePrice);

            mercariDriver.Quit();

            return newPrice;
        }

        public String ScrapeEBid(string searchTerm, bool shouldBeSearched)
        {
            if (shouldBeSearched)
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
            else
            {
                return "";
            }
            
        }

        public string ScrapeImageByEbidItem(string itemLink)
        {
            try
            {
                eBidDriver = new ChromeDriver(options);

                eBidDriver.Navigate().GoToUrl(itemLink);

                IWebElement searchSpace = eBidDriver.FindElement(By.XPath("//div[@class='slider-wrap']"));
                string image = searchSpace.FindElement(By.TagName("a")).GetAttribute("href");

                mercariDriver.Quit();

                return image;
            }
            catch { return ""; }

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

        public String ScrapePoshmark(string searchTerm, bool shouldBeSearched)
        {
            if (shouldBeSearched)
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
            else
            {
                return "";
            }
           
        }

        public string ScrapeImageByPoshmarkItem(string itemLink)
        {
            try
            {
                poshmarkDriver = new ChromeDriver(options);

                poshmarkDriver.Navigate().GoToUrl(itemLink);

                IWebElement searchSpace = poshmarkDriver.FindElement(By.XPath("//div[@class='carousel__inner']"));
                string image = searchSpace.FindElement(By.TagName("img")).GetAttribute("src");

                poshmarkDriver.Quit();

                return image;
            }
            catch { return ""; }

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

        public String ScrapeEcrater(string searchTerm, bool shouldBeSearched)
        {
            if (shouldBeSearched)
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
            else
            {
                return "";
            }
           
        }

        public string ScrapeImageByEcraterItem(string itemLink)
        {
            try
            {
                eCraterDriver = new ChromeDriver(options);

                eCraterDriver.Navigate().GoToUrl(itemLink);

                IWebElement searchSpace = eCraterDriver.FindElement(By.XPath("//div[@id='wBiggerImage']"));
                string image = searchSpace.FindElement(By.TagName("a")).GetAttribute("data-image");

                eCraterDriver.Quit();

                return image;
            }
            catch { return ""; }
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
