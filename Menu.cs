//==========================================================
// Student Number : S10275159
// Student Name : Aw Chong Boon
// Partner Name : Yip Jie Shen
//==========================================================

using System;
using System.Collections.Generic;

namespace S10275159_PRG2Assignment
{
    public class Menu
    {
        private string menuID;
        private string menuName;
        private List<FoodItem> foodItems;

        public string MenuID
        {
            get { return menuID; }
            set { menuID = value; }
        }

        public string MenuName
        {
            get { return menuName; }
            set { menuName = value; }
        }

        public List<FoodItem> FoodItems // Use FoodItems to match usual conventions, or just call it 'Items'
        {
            get { return foodItems; }
            set { foodItems = value; }
        }

        public Menu() 
        { 
            foodItems = new List<FoodItem>();
            menuID = "";
            menuName = "";
        }

        public Menu(string id, string name)
        {
            menuID = id;
            menuName = name;
            foodItems = new List<FoodItem>();
        }

        public void AddFoodItem(FoodItem item)
        {
            foodItems.Add(item);
        }

        public bool RemoveFoodItem(FoodItem item)
        {
            if (foodItems.Contains(item))
            {
                foodItems.Remove(item);
                return true;
            }
            return false;
        }

        public void DisplayFoodItems()
        {
            foreach(var item in foodItems)
            {
                Console.WriteLine(item.ToString());
            }
        }

        public override string ToString()
        {
            return $"Menu: {MenuName}, Items: {foodItems.Count}";
        }
    }
}
