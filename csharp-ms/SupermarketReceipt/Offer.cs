namespace SupermarketReceipt
{
    public enum SpecialOfferCategories
    {
        BuyItemsGetItemsFree,
        SpecificPercentDiscount,
        ItemsForSales,
        NoDiscount
    }


    public class Offer
    {
        private Product _product;

        public Offer(SpecialOfferItem offerType, Product product)
        {
            OfferType = offerType;
            _product = product;
        }

        public SpecialOfferItem OfferType { get; }
        public Product Product { get; }
    }

}