using PRN211_Grocery_store.Data;
using PRN211_Grocery_store.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PRN211_Grocery_store.Models.DAO
{
    public class ProductDAO
    {
        private static ProductDAO instance = null;
        private static readonly object instanceLock = new object();
        public static ProductDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null) instance = new ProductDAO();
                    return instance;
                }
            }
        }

        public List<Product> GetProducts()
        {
            List<Product> products = new();
            try
            {
                using (var context = new ApplicationDBContext()) {
                    products = context.Products.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return products;
        }

        public Product GetProductById(int productId)
        {
            Product product = null;
            try
            {
                using (var context = new ApplicationDBContext())
                {
                    product = context.Products.Find(productId);
                }
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return product;
        }

        public void Update(Product product)
        {
            try
            {
                using (var context = new ApplicationDBContext())
                {
                    context.Products.Update(product);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
