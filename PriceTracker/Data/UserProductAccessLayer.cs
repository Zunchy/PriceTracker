using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PriceTracker.Data
{
    public interface IUserProductAccessLayer
    {
        IEnumerable GetAllUserProducts();
    }

    public class UserProductAccessLayer : IUserProductAccessLayer
    {
        private ApplicationDbContext _context;
        public UserProductAccessLayer(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable GetAllUserProducts()
        {
            try
            {
                return _context.UserProduct.ToList();
            }
            catch
            {
                throw;
            }
        }
    }
}
