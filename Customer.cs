    //==========================================================
// Student Number : S10275159
// Student Name : Aw Chong Boon
// Partner Name : Yip Jie Shen
//==========================================================

using System;
using System.Collections.Generic;

namespace S10275159_PRG2Assignment
{
    public class Customer
    {
        private string name;
        private string email;
        private List<Order> orderHistory = new List<Order>();
        private List<FavouriteOrder> favouriteOrders = new List<FavouriteOrder>();

        public List<FavouriteOrder> FavouriteOrders
        {
            get { return favouriteOrders; }
            set { favouriteOrders = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public List<Order> OrderHistory
        {
            get { return orderHistory; }
            set { orderHistory = value; }
        }

        public Customer() 
        {
            name = "Unknown";
            email = "";
        }

        public Customer(string name, string email) : this()
        {
            Name = name;
            Email = email;
        }

        public override string ToString()
        {
            return $"Name: {name}, Email: {email}";
        }

    }
}
