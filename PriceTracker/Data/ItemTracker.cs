using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PriceTracker.Data
{
    public class ItemTracker
    {
        private DataAccess dataAccesser; 

        public ItemTracker()
        {
            dataAccesser = new DataAccess();
        }
        public void CreateTrackedItemRelation(string user, string itemIdentifier)
        {
            dataAccesser.CreateTrackedItem(user, itemIdentifier);
        }

    }
}
