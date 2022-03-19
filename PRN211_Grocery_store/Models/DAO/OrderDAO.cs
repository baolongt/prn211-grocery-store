using PRN211_Grocery_store.Data.Entity;
using PRN211_Grocery_store.Data;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
namespace PRN211_Grocery_store.Models.DAO
{
    public class OrderDAO
    {
        private static OrderDAO instance = null;
        private static readonly object instanceLock = new();
        public static OrderDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null) instance = new OrderDAO();
                    return instance;
                }
            }
        }

        public List<Order> GetOrderByUsername(string username)
        {
            List<Order> orders = new List<Order>();
            try
            {
                using (var context = new ApplicationDBContext())
                {
                    orders = context.Orders.Include(c => c.OrderDetails).Where(o => o.Username.Equals(username)).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return orders;
        }

        public List<Order> GetAll ()
        {
            List<Order> orders = new();
            try
            {
                using (var context = new ApplicationDBContext())
                { 
                    orders = context.Orders.Include(c => c.OrderDetails).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return orders;
        }

        public Order GetOrderByID(int orderID)
        {
            Order order = null;
            try
            {
                using (var context = new ApplicationDBContext())
                {
                    order = context.Orders
                        .Include(c => c.OrderDetails)
                        .ThenInclude(od => od.Product)
                        .SingleOrDefault(m => m.Id == orderID);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return order;
        }

        public async void AddNew(Order order)
        {
            try
            {
                Order _order = instance.GetOrderByID(order.Id);
                if (_order == null)
                {
                    using var context = new ApplicationDBContext();
                    context.Orders.Add(order);
                    context.SaveChanges();
                }
                else throw new Exception("The order is already existed!");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
