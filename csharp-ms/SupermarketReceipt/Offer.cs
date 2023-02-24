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

        public Offer(SpecialOfferCategories offerType, Product product, int sizeOfGrouping, double discountRate, double sellingPrice)
        {
            OfferType = offerType;
            SizeOfGrouping = sizeOfGrouping;
            DiscountRate = discountRate;
            SellingPrice = sellingPrice;
            _product = product;
        }

        public SpecialOfferCategories OfferType { get; }
        public int SizeOfGrouping { get; }
        public double DiscountRate { get; }
        public double SellingPrice { get; }
    }
}