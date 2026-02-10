//==========================================================
// Student Number : S10275159
// Student Name : Aw Chong Boon
// Partner Name : Yip Jie Shen
//==========================================================

using System.Collections.Generic;

namespace S10275159_PRG2Assignment
{
    public class FavouriteOrder
    {
        public int FavouriteId { get; set; }
        public string FavouriteName { get; set; } = "";
        public string RestaurantId { get; set; } = "";
        public List<FavouriteOrderItem> Items { get; set; } = new List<FavouriteOrderItem>();

        public FavouriteOrder() { }

        public FavouriteOrder(int favId, string favName, string restaurantId)
        {
            FavouriteId = favId;
            FavouriteName = favName;
            RestaurantId = restaurantId;
        }

        public override string ToString()
        {
            return $"[{FavouriteId}] {FavouriteName} (Restaurant: {RestaurantId}, Items: {Items.Count})";
        }
    }
}
