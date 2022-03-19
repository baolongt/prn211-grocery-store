using PRN211_Grocery_store.Data.Entity;
using System.Collections.Generic;
using PRN211_Grocery_store.Models.DAO;
namespace PRN211_Grocery_store.Models.Repository
{
    public class OrderRepository : IOrderRepository
    {
        public void AddNew(Order order) => OrderDAO.Instance.AddNew(order);
        public Order Get(int id) => OrderDAO.Instance.GetOrderByID(id);
        public IList<Order> GetAll() => OrderDAO.Instance.GetAll();
        public IList<Order> GetOrderByUsername(string username) => OrderDAO.Instance.GetOrderByUsername(username);
    }
}
