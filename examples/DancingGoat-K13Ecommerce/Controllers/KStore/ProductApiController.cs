using DancingGoat.Models.WebPage.K13Store.ProductPage;

using Kentico.Xperience.K13Ecommerce.Products;
using Kentico.Xperience.K13Ecommerce.StoreApi;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace DancingGoat.Controllers.KStore;

[Route("api/k13/product")]
public class ProductApiController : Controller
{
    private readonly IProductService productService;


    public ProductApiController(IProductService productService)
    {
        this.productService = productService;
    }


    [HttpGet("inventory-price-info")]
    public async Task<VariantInventoryPriceInfo> GetVariantInfo([FromQuery] int variantSkuId, [FromServices] IStringLocalizer<SharedResources> localizer)
    {
        var info = await productService.GetVariantInventoryPriceInfo(variantSkuId);

        bool isInStock = (info.SkuTrackInventory == TrackInventoryTypeEnum.Disabled) || (info.SkuAvailableItems > 0);
        bool allowSale = isInStock || !info.SkuSellOnlyAvailable;

        var prices = info.Prices;
        var currency = info.Prices.Currency;
        decimal discount = prices.StandardPrice - prices.Price;

        string priceSavings = string.Empty;
        if (discount > 0)
        {
            decimal discountPercentage = Math.Round(discount * 100 / prices.StandardPrice);
            string formattedDiscount = string.Format(currency.CurrencyFormatString, discount);
            priceSavings = $"{formattedDiscount} ({discountPercentage}%)";
        }

        var response = new VariantInventoryPriceInfo
        {
            TotalPrice = string.Format(currency.CurrencyFormatString, prices.Price),
            BeforeDiscount = discount > 0 ? string.Format(currency.CurrencyFormatString, prices.StandardPrice) : string.Empty,
            Savings = priceSavings,
            IsInStock = isInStock,
            AllowSale = allowSale,
            VariantSKUID = variantSkuId,
            StockMessage = isInStock ? localizer["In stock"].Value : localizer["Out of stock"].Value,
        };

        return response;
    }
}
