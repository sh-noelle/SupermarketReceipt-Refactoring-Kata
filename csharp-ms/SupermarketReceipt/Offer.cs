namespace SupermarketReceipt
{
    public enum SpecialOfferType
    {
        ThreeForTwo,
        TenPercentDiscount,
        TwentyPercentDiscount,
        TwoForAmount,
        FiveForAmount,
        GetOneFree,
    }

    public enum SpecialOfferCategories
    {
        BuyItemsGetItemsFree,
        SpecificPercentDiscount,
        AmountForSpecificPrice,
    }

    public class Offer
    {
        private Product _product;

        public Offer(SpecialOfferType offerType, Product product, double argument)
        {
            OfferType = offerType;
            Argument = argument;
            _product = product;
        }

        public SpecialOfferType OfferType { get; }
        public double Argument { get; }
    }
}