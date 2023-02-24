using System;
using System.Collections.Generic;
using System.Text;

namespace SupermarketReceipt
{
    public class SpecialOfferItem
    {
        public string SpecialOffer { get; set; }
        public SpecialOfferCategories Categories { get; set;}
        public int SizeOfGrouping { get; set; }
        public double DiscountRate { get; set; }
    }
}
