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
            List<Product> products = new List<Product>();
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
    }
}
