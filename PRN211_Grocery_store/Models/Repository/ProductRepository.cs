using PRN211_Grocery_store.Data.Entity;
using PRN211_Grocery_store.Models.DAO;
using System.Collections.Generic;

namespace PRN211_Grocery_store.Models.Repository
{
    public class ProductRepository : IProductRepository
    {
        public Product GetProductById(int id) => ProductDAO.Instance.GetProductById(id);

        public IList<Product> GetProducts() => ProductDAO.Instance.GetProducts();
    }
}
