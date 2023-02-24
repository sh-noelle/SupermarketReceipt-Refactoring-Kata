namespace SupermarketReceipt
{
    public enum SpecialOffers
    {
        BuyItemsGetItemsFree,
        TenPercentDiscount,
        TwentyPercentDiscount,
        TwoItemsForSpecificPrice,
        FiveItemsForSpecificPrice,
        NoDiscount
    }

    public class Offer
    {
        private Product _product;

        public Offer(SpecialOffers offerType, Product product, int sizeOfGrouping, double discountRate, double sellingPrice)
        {
            OfferType = offerType;
            SizeOfGrouping = sizeOfGrouping;
            DiscountRate = discountRate;
            SellingPrice = sellingPrice;
            _product = product;
        }

        public SpecialOffers OfferType { get; }
        public int SizeOfGrouping { get; }
        public double DiscountRate { get; }
        public double SellingPrice { get; }
    }
}