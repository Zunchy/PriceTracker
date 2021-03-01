using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PriceTracker.Data
{
    public class DataFormater
    {

        public List<DisplayItem> FormatMercari(string data)
        {
            List<DisplayItem> returnList = new List<DisplayItem>();

            List<String> dataItems = data.Split("END ITEM").ToList();

            foreach(String item in dataItems)
            {
                List<String> itemInfo = item.Split("\r\n").ToList();
                if(itemInfo.Count > 1)
                {
                    DisplayItem newItem = new DisplayItem();

                    newItem.ItemName = itemInfo.First();

                    List<String> priceInfo = itemInfo.Find(x => x.Contains('$')).Split(' ').ToList();

                    if (!priceInfo.First().Contains('$'))
                    {
                        priceInfo.Remove(priceInfo.First());
                    }

                    newItem.ItemPrice = Double.Parse(priceInfo.First().Substring(1, priceInfo.First().Length - 1));

                    newItem.ItemLink = itemInfo.Last();

                    returnList.Add(newItem);
                }

            }

            return returnList;
        }

        public List<DisplayItem> FormatEBid(string data)
        {
            List<DisplayItem> returnList = new List<DisplayItem>();

            List<String> dataItems = data.Split("END ITEM").ToList();

            foreach (String item in dataItems)
            {
                List<String> itemInfo = item.Split("\r\n").ToList();
                if (itemInfo.Count > 1)
                {
                    DisplayItem newItem = new DisplayItem();

                    newItem.ItemName = itemInfo.First();

                    newItem.ItemPrice = Double.Parse(itemInfo[1].Substring(1, itemInfo[1].Length - 1));

                    newItem.ItemLink = itemInfo.Last();

                    returnList.Add(newItem);
                }

            }

            return returnList;
        }
    }
}
