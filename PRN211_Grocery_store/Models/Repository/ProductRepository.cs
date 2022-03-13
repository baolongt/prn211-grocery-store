using PRN211_Grocery_store.Data.Entity;
using PRN211_Grocery_store.Models.DAO;
using System.Collections.Generic;

namespace PRN211_Grocery_store.Models.Repository
{
    public class ProductRepository : IProductRepository
    {
        public IList<Product> GetProducts() => ProductDAO.Instance.GetProducts();
    }
}
