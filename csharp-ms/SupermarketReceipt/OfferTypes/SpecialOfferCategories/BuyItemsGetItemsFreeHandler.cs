using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace SupermarketReceipt.OfferTypes.SpecialOfferCategories
{
    public class BuyItemsGetItemsFreeHandler : OfferHandlerBase
    {
        public override void HandleOffer(Receipt receipt, Dictionary<Product, Offer> specialOffers, Product product, double quantity, SupermarketCatalog catalog)
        {
            var offer = specialOffers[product];
            if (offer.OfferType == SpecialOffers.BuyItemsGetItemsFree)
            {
                var unitPrice = catalog.GetUnitPrice(product);
                var groupedProduct = Math.Floor(quantity / offer.SizeOfGrouping);
                var nonGroupedProduct = quantity % offer.SizeOfGrouping;

                var markedPrice = unitPrice * quantity;
                var totalPrice = unitPrice * groupedProduct + unitPrice * nonGroupedProduct;
                var discount = markedPrice - totalPrice;
                var discountStatement = new DiscountStatement(
                    product,
                    $"{offer.SizeOfGrouping} for {offer.SellingPrice}",
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
