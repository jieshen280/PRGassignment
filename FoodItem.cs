//==========================================================
// Student Number : S10275159
// Student Name : Aw Chong Boon
// Partner Name : Yip Jie Shen
//==========================================================

using System;

namespace S10275159_PRG2Assignment
{
    public class FoodItem
    {
        private string itemName;
        private string description;
        private double price;
        private string customise;

        public string ItemName
        {
            get { return itemName; }
            set { itemName = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public double Price
        {
            get { return price; }
            set { price = value; }
        }

        public string Customise
        {
            get { return customise; }
            set { customise = value; }
        }

        public FoodItem() 
        {
            itemName = "";
            description = "";
            customise = "";
        }

        public FoodItem(string itemName, string description, double price) : this()
        {
            ItemName = itemName;
            Description = description;
            Price = price;
            Customise = "";
        }

        public override string ToString()
        {
            return $"{itemName}: {description} - ${price:F2}";
        }
    }
}
