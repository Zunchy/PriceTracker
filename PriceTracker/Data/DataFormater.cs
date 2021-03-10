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

                    itemInfo.Remove(itemInfo.First());

                    List<String> priceInfo = itemInfo.Find(x => x.Contains('$')).Split(' ').ToList();

                    if (!priceInfo.First().Contains('$'))
                    {
                        priceInfo.Remove(priceInfo.First());
                    }

                    newItem.ItemPrice = Double.Parse(priceInfo.First().Substring(1, priceInfo.First().Length - 1));

                    newItem.ItemImage = itemInfo.Last();

                    itemInfo.Remove(itemInfo.Last());

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

                    itemInfo.Remove(itemInfo.First());

                    newItem.ItemPrice = Double.Parse(itemInfo.First().Substring(1, itemInfo.First().Length - 1));

                    newItem.ItemImage = itemInfo.Last();

                    itemInfo.Remove(itemInfo.Last());

                    newItem.ItemLink = itemInfo.Last();

                    returnList.Add(newItem);
                }

            }

            return returnList;
        }

        public List<DisplayItem> FormatPoshmark(string data)
        {
            List<DisplayItem> returnList = new List<DisplayItem>();

            List<String> dataItems = data.Split("END ITEM").ToList();

            foreach (String item in dataItems)
            {
                List<String> itemInfo = item.Split("\r\n").ToList();
                if (itemInfo.Count > 3)
                {
                    DisplayItem newItem = new DisplayItem();

                    newItem.ItemName = itemInfo.First();

                    itemInfo.Remove(itemInfo.First());

                    List<String> priceInfo = itemInfo.Find(x => x.Contains('$')).Split(' ').ToList();

                    newItem.ItemPrice = Double.Parse(priceInfo.First().Substring(1, priceInfo.First().Length - 1));

                    newItem.ItemImage = itemInfo.Last();

                    itemInfo.Remove(itemInfo.Last());

                    newItem.ItemLink = itemInfo.Last();

                    returnList.Add(newItem);
                }

            }

            return returnList;
        }

        public List<DisplayItem> FormatEcrater(string data)
        {
            List<DisplayItem> returnList = new List<DisplayItem>();

            List<String> dataItems = data.Split("END ITEM").ToList();

            foreach (String item in dataItems)
            {
                List<String> itemInfo = item.Split("\r\n").ToList();
                if (itemInfo.Count > 1)
                {
                    DisplayItem newItem = new DisplayItem();

                    newItem.ItemName = itemInfo[1];

                    newItem.ItemPrice = Double.Parse(itemInfo.First().Substring(1, itemInfo.First().Length - 1));

                    newItem.ItemImage = itemInfo.Last();

                    itemInfo.Remove(itemInfo.Last());

                    newItem.ItemLink = itemInfo.Last();

                    returnList.Add(newItem);
                }

            }

            return returnList;
        }
    }
}
