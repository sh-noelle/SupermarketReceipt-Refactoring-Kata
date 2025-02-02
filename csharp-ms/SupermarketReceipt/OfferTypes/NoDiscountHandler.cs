﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace SupermarketReceipt.OfferTypes
{
    public class NoDiscountHandler : OfferHandlerBase
    {
        public override void HandleOffer(Receipt receipt, Dictionary<Product, Offer> specialOffers, Product product, double quantity, SupermarketCatalog catalog)
        {
            var offer = specialOffers[product];
            if (offer.OfferType == SupermarketReceipt.SpecialOffers.NoDiscount) 
            {
                var discount = 0;
                var discountStatement = new DiscountStatement(
                        product,
                        "Marked price",
                        discount
                        );
                receipt.AddDiscountStatement(discountStatement);
                if (nextHandler != null) 
                {
                    nextHandler.HandleOffer(receipt, specialOffers, product, quantity, catalog);
                }
            }
            else if(nextHandler != null)
            {
                nextHandler.HandleOffer(receipt, specialOffers, product, quantity, catalog);
            }
        }
    }
}
