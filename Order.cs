//==========================================================
// Student Number : S10275159
// Student Name : Aw Chong Boon
// Partner Name : Yip Jie Shen
//==========================================================

using System;
using System.Collections.Generic;

namespace S10275159_PRG2Assignment
{
    public class Order
    {
        private int orderID;
        private DateTime orderDateTime; 
        private double orderTotal; // Was Amount
        private string orderStatus; // Was Status
        private DateTime deliveryDateTime;
        private string deliveryAddress;
        private string orderPaymentMethod;
        private bool orderPaid;

        private List<OrderedFoodItem> orderItems = new List<OrderedFoodItem>();

        public int OrderID
        {
            get { return orderID; }
            set { orderID = value; }
        }

        public DateTime OrderDateTime
        {
            get { return orderDateTime; }
            set { orderDateTime = value; }
        }

        public double OrderTotal
        {
            get { return orderTotal; }
            set { orderTotal = value; }
        }

        public string OrderStatus
        {
            get { return orderStatus; }
            set { orderStatus = value; }
        }

        public DateTime DeliveryDateTime
        {
            get { return deliveryDateTime; }
            set { deliveryDateTime = value; }
        }

        public string DeliveryAddress
        {
            get { return deliveryAddress; }
            set { deliveryAddress = value; }
        }

        public string OrderPaymentMethod
        {
            get { return orderPaymentMethod; }
            set { orderPaymentMethod = value; }
        }

        public bool OrderPaid
        {
            get { return orderPaid; }
            set { orderPaid = value; }
        }

        public List<OrderedFoodItem> OrderItems
        {
            get { return orderItems; }
            set { orderItems = value; }
        }

        public Order() 
        {
            deliveryAddress = "";
            orderStatus = "Pending";
            orderPaymentMethod = "";
            orderDateTime = DateTime.Now;
        }

        public Order(int orderId, DateTime deliveryDateTime)
        {
            orderID = orderId;
            DeliveryDateTime = deliveryDateTime;
            orderStatus = "Pending";
            deliveryAddress = "";
            orderPaymentMethod = "";
            orderDateTime = DateTime.Now;
        }

        public double CalculateOrderTotal()
        {
            double total = 0;
            foreach(var item in orderItems)
            {
                total += item.CalculateSubtotal();
            }
            return total;
        }

        public void AddOrderedFoodItem(OrderedFoodItem item)
        {
            orderItems.Add(item);
        }

        public bool RemoveOrderedFoodItem(OrderedFoodItem item)
        {
             if (orderItems.Contains(item))
             {
                 orderItems.Remove(item);
                 return true;
             }
             return false;
        }

        public void DisplayOrderedFoodItems()
        {
             foreach(var item in orderItems)
             {
                 Console.WriteLine(item.ToString());
             }
        }

        public override string ToString()
        {
            return $"OrderID: {orderID}, Status: {orderStatus}, Total: {orderTotal:F2}, Date: {deliveryDateTime}, Payment: {orderPaymentMethod}";
        }
    }
}
