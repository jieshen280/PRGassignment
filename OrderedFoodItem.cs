//==========================================================
// Student Number : S10275159
// Student Name : Aw Chong Boon
// Partner Name : Yip Jie Shen
//==========================================================

using System;

namespace S10275159_PRG2Assignment
{
    public class OrderedFoodItem : FoodItem
    {
        private int qtyOrdered;
        private double subTotal;

        public int QtyOrdered
        {
            get { return qtyOrdered; }
            set { qtyOrdered = value; }
        }

        public double SubTotal
        {
            get { return subTotal; }
            // No setter, calculated
        }

        public OrderedFoodItem() : base() { }

        public OrderedFoodItem(string name, string desc, double price, int qty)
            : base(name, desc, price)
        {
            QtyOrdered = qty;
            CalculateSubtotal();
        }

        // Constructor to convert existing FoodItem
        public OrderedFoodItem(FoodItem item, int qty)
            : base(item.ItemName, item.Description, item.Price)
        {
            QtyOrdered = qty;
            CalculateSubtotal();
        }

        public double CalculateSubtotal()
        {
            subTotal = Price * QtyOrdered;
            return subTotal;
        }

        public override string ToString()
        {
             string cust = string.IsNullOrEmpty(Customise) ? "" : $" [{Customise}]";
             return $"{base.ItemName}{cust} - {QtyOrdered}";
        }

    }
}
