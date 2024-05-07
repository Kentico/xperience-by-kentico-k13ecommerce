using CMS.ContentEngine;

using Kentico.Xperience.K13Ecommerce.StoreApi;

namespace DancingGoat.Models
{
    public class CartContentViewModel
    {
        private readonly string currencyFormatString;


        public decimal TotalTax { get; set; }


        public decimal TotalShipping { get; set; }


        public decimal GrandTotal { get; set; }


        public bool IsEmpty { get; set; }


        public decimal RemainingAmountForFreeShipping { get; set; }


        public IEnumerable<string> AppliedCouponCodes { get; set; }


        public IEnumerable<CartItemViewModel> CartItems { get; set; }


        public CartContentViewModel(KShoppingCartContent cart, ILookup<int, ContentItemAsset> productsImages)
        {
            TotalTax = cart.TotalTax;
            TotalShipping = cart.TotalShipping;
            GrandTotal = cart.GrandTotal;
            currencyFormatString = cart.Currency.CurrencyFormatString;
            IsEmpty = cart.CartProducts!.Count == 0;
            RemainingAmountForFreeShipping = cart.RemainingAmountForFreeShipping;
            AppliedCouponCodes = cart.CouponCodes?.Select(x => x.Code) ?? [];

            CartItems = cart.CartProducts.Select((cartItemInfo) =>
            {
                return new CartItemViewModel
                {
                    CartItemID = cartItemInfo.CartItemId,
                    CartItemUnits = cartItemInfo.CartItemUnits,
                    SKUID = cartItemInfo.VariantSKU?.Skuid ?? cartItemInfo.ProductSKU.Skuid,
                    ParentSKUID = cartItemInfo.VariantSKU != null ? cartItemInfo.ProductSKU.Skuid : null,
                    SKUImagePath = productsImages[cartItemInfo.ProductSKU.Skuid].First().Url,
                    SKUName = cartItemInfo.VariantSKU?.SkuName ?? cartItemInfo.ProductSKU.SkuName,
                    TotalPrice = cartItemInfo.TotalPrice
                };
            });
        }


        public string FormatPrice(decimal price) => string.Format(currencyFormatString, price);
    }
}
