//==========================================================
// Student Number : S10275159
// Student Name : Aw Chong Boon
// Partner Name : Yip Jie Shen
//==========================================================

using System;
using System.Collections.Generic;

namespace S10275159_PRG2Assignment
{
    public class Restaurant
    {
        private string name;
        private string email;
        private string restaurantID;
        private Menu menu = new Menu(); // Keep object init
        private Queue<Order> orderQueue = new Queue<Order>();
        private List<SpecialOffer> specialOffers = new List<SpecialOffer>();

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

        public string RestaurantID
        {
            get { return restaurantID; }
            set { restaurantID = value; }
        }

        public Menu Menu
        {
            get { return menu; }
            set { menu = value; }
        }

        public Queue<Order> OrderQueue
        {
            get { return orderQueue; }
            set { orderQueue = value; }
        }

        public List<SpecialOffer> SpecialOffers
        {
            get { return specialOffers; }
            set { specialOffers = value; }
        }

        public Restaurant()
        {
            name = "";
            email = "";
            restaurantID = "";
        }

        public Restaurant(string name, string email, string restaurantId) : this()
        {
            Name = name;
            Email = email;
            RestaurantID = restaurantId;
        }

        public override string ToString()
        {
            return $"Restaurant: {name} ({restaurantID})";
        }

        public void DisplayOrders()
        {
            foreach (var o in orderQueue)
            {
                Console.WriteLine(o.ToString());
            }
        }

        public void DisplaySpecialOffers()
        {
            if (specialOffers.Count == 0)
            {
                Console.WriteLine("No special offers available.");
            }
            else
            {
                Console.WriteLine($"Special Offers for {Name}:");
                int i = 1;
                foreach(var offer in specialOffers)
                {
                    Console.WriteLine($"{i}. {offer}");
                    i++;
                }
            }
        }

        public void DisplayMenu()
        {
            menu.DisplayFoodItems();
        }

        public void AddMenu(Menu m)
        {
            menu = m;
        }

        public bool RemoveMenu(Menu m)
        {
            if (menu == m)
            {
                menu = new Menu();
                return true;
            }
            return false;
        }
    }
}
