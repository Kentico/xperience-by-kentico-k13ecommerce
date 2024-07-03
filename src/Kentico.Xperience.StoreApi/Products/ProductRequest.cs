using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

using Kentico.Xperience.StoreApi.Currencies;

namespace Kentico.Xperience.StoreApi.Products;

public class ProductRequest
{
    /// <summary>
    /// To determine if product has its page, culture needs to be provided
    /// so documents in the specific culture will be checked.
    /// </summary>
    [RegularExpression("[a-zA-Z]{2}-[a-zA-Z]{2}")]
    public string Culture { get; set; }

    /// <summary>
    /// Product currency.
    /// </summary>
    [CurrencyValidation]
    public string Currency { get; set; }

    /// <summary>
    /// Order by SQL expression.
    /// </summary>
    public string OrderBy { get; set; }

    /// <summary>
    /// Limit how many products to return.
    /// </summary>
    [DefaultValue(12)]
    [Range(1, 1000)]
    public int Limit { get; set; }

    /// <summary>
    /// If true variants are loaded too for products with variants (default false).
    /// </summary>
    public bool WithVariants { get; set; }

    public void Deconstruct(out string culture, out string currency, out string orderBy,
        out int limit, out bool withVariants)
    {
        culture = Culture;
        currency = Currency;
        orderBy = OrderBy;
        limit = Limit;
        withVariants = WithVariants;
    }
}
