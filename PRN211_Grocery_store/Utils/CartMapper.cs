using System.Collections.Generic;
using PRN211_Grocery_store.Data.Entity;
using PRN211_Grocery_store.Models.ViewModel;

namespace PRN211_Grocery_store.Utils
{
    public class CartMapper
    {
        private static CartMapper instance = null;
        private static readonly object instanceLock = new object();
        public static CartMapper Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null) instance = new CartMapper();
                    return instance;
                }
            }
        }

        public List<OrderDetail> MapToOrderDetail(List<Item> cart)
        {
            List<OrderDetail> orderDetailList = new List<OrderDetail>();
            foreach (Item item in cart)
            {
                OrderDetail orderDetail = new OrderDetail()
                {
                    ProductId = item.Product.Id,
                    Quantity = item.Quantity,
                    Price = item.Product.Price
                };
                orderDetailList.Add(orderDetail);
            }
            return orderDetailList;
        }
    }
}
