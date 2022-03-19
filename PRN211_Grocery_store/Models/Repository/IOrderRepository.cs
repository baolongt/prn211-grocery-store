using System.Collections.Generic;
using PRN211_Grocery_store.Data.Entity;
namespace PRN211_Grocery_store.Models.Repository
{
    public interface IOrderRepository
    {
        public IList<Order> GetAll();
        public Order Get(int id);
        public void AddNew(Order order);
        public IList<Order> GetOrderByUsername(string username);
    }
}
