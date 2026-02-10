//==========================================================
// Student Number : S10275159
// Student Name : Aw Chong Boon
// Partner Name : Yip Jie Shen
//==========================================================

using System;

namespace S10275159_PRG2Assignment
{
    public class SpecialOffer
    {
        private string offerCode;
        private string offerDesc;
        private double discount = 0;

        public string OfferCode
        {
            get { return offerCode; }
            set { offerCode = value; }
        }

        public string OfferDesc
        {
            get { return offerDesc; }
            set { offerDesc = value; }
        }

        public double Discount
        {
            get { return discount; }
            set { discount = value; }
        }

        public SpecialOffer()
        {
            offerCode = "";
            offerDesc = "";
        }

        public SpecialOffer(string code, string desc, double discount)
        {
            offerCode = code;
            offerDesc = desc;
            this.discount = discount;
        }

        public override string ToString()
        {
                return $"{OfferCode}: {OfferDesc} ({Discount}%)";
        }
    }
}
