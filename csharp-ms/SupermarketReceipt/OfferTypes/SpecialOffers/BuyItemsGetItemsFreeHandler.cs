﻿using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace SupermarketReceipt.OfferTypes.SpecialOffers
{
    public class BuyItemsGetItemsFreeHandler : OfferHandlerBase
    {
        public override void HandleOffer(Receipt receipt, Dictionary<Product, Offer> specialOffers, Product product, int quantity, SupermarketCatalog catalog)
        {
            var offer = specialOffers[product];
            if (offer.OfferType == SpecialOfferCategories.BuyItemsGetItemsFree)
            {
                var unitPrice = catalog.GetUnitPrice(product);
                var groupedProduct = quantity / offer.SizeOfGrouping;
                var nonGroupedProduct = quantity % offer.SizeOfGrouping;

                var markedPrice = unitPrice * quantity;
                var totalPrice = unitPrice * groupedProduct + unitPrice * nonGroupedProduct;
                var discount = markedPrice - totalPrice;
                var discountStatement = new DiscountStatement(
                    product,
                    $"{offer.SizeOfGrouping} for {offer.SellingPrice}",
                    discount * -1
                    );

                receipt.AddDiscountStatement(discountStatement);
            }
            else
            {
                next.HandleOffer(receipt, specialOffers, product, quantity, catalog);
            }
        }
    }
}