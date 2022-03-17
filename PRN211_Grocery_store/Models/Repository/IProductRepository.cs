
using PRN211_Grocery_store.Data.Entity;
using System.Collections.Generic;

namespace PRN211_Grocery_store.Models.Repository
{
    public interface IProductRepository
    {
        public IList<Product> GetProducts();
        public Product GetProductById(int id);
    }
}
