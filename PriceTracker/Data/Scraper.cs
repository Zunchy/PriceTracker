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

        public String ScrapeMercari()
        {

            //mercari, eBid, ecrater, rubylane


            // Search For Products
  
            driver.Navigate().GoToUrl("https://www.mercari.com/search/?keyword=dragonball");
            //IReadOnlyCollection<IWebElement> test = driver.FindElements(By.TagName("li"));
            IReadOnlyCollection<IWebElement> searchSpace = driver.FindElements(By.XPath("//div[@data-testid=\"ItemContainer\"]"));

            String result = "";
            foreach(IWebElement item in searchSpace)
            {
                IWebElement itemContent = item.FindElement(By.XPath("./.."));
                String link = itemContent.GetAttribute("href");

                result += itemContent.Text;
                result += "\r\n";
                result += link;
                result += "END ITEM";
    
            }

            return result;

            /*
            // Get Product Information 
            using (IWebDriver driver = new ChromeDriver(options))
            {

                driver.Navigate().GoToUrl("https://www.mercari.com/us/item/m22856012925/");
                IWebElement test = driver.FindElement(By.XPath("//*[contains(@class, 'RightColumn')]"));

                String result = "";

                result = test.Text;

                return result;
            }
            */


        }

    }
}
