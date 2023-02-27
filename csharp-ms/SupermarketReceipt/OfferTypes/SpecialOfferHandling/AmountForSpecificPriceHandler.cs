using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace SupermarketReceipt.OfferTypes.SpecialOfferHandling
{
    public class AmountForSpecificPriceHandler : OfferHandlerBase
    {
        public override void HandleOffer(Receipt receipt, Dictionary<Product, Offer> specialOffers, Product product, double quantity, SupermarketCatalog catalog)
        {
            var offer = specialOffers[product];
            if (offer.OfferType.Categories == SpecialOfferCategories.ItemsForSales)
            {
                var unitPrice = catalog.GetUnitPrice(product);
                var groupedProduct = Math.Floor(quantity / offer.OfferType.SizeOfGrouping);
                var nonGroupedProduct = quantity % offer.OfferType.SizeOfGrouping;
                var totalPrice = offer.SellingPrice * groupedProduct + unitPrice * nonGroupedProduct;
                var markedPrice = unitPrice * quantity;
                var discount = markedPrice - totalPrice;

                var discountStatement = new DiscountStatement(
                    product,
                    $"{offer.OfferType.SizeOfGrouping} for {offer.SellingPrice}",
                    discount
                    );

                receipt.AddDiscountStatement(discountStatement);
                PasstoNextHandler(receipt, specialOffers, product, quantity, catalog);
            }
            else 
            {
                PasstoNextHandler(receipt, specialOffers, product, quantity, catalog);
            }
        }
    }
}
