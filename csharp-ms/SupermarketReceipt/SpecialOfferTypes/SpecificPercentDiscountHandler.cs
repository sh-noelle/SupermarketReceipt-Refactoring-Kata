using System;
using System.Collections.Generic;
using System.Text;

namespace SupermarketReceipt.SpecialOfferTypes
{
    public class SpecificPercentDiscountHandler:OfferHandler
    {
        public override void HandleOffer(Receipt receipt, Product product, int quantity, SupermarketCatalog catalog)
        {
            if () 
            {
            }
            else 
            {
                next.HandleOffer(receipt, product, quantity, catalog); 
            }
        }
    }
}
