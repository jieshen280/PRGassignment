//==========================================================
// Student Number : S10275159
// Student Name : Aw Chong Boon
// Partner Name : Yip Jie Shen
//==========================================================

using System;

namespace S10275159_PRG2Assignment
{
    public class FavouriteOrderItem
    {
        public string ItemName { get; set; } = "";
        public int Qty { get; set; } = 0;
        public string Customise { get; set; } = "";

        public FavouriteOrderItem() { }

        public FavouriteOrderItem(string itemName, int qty, string customise = "")
        {
            ItemName = itemName;
            Qty = qty;
            Customise = customise;
        }

        public override string ToString()
        {
            string cust = string.IsNullOrWhiteSpace(Customise) ? "" : $" ({Customise})";
            return $"{ItemName} x{Qty}{cust}";
        }
    }
}
