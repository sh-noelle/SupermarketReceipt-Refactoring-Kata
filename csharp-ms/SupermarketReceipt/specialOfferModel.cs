using System;
using System.Collections.Generic;
using System.Text;

namespace SupermarketReceipt
{
    public class SpecialOfferModel
    {
        public SpecialOfferCategories OfferType { get; set; }
        public int GroupingNumber { get; set;}
        public string Labelling { get; set;}
    }
}
