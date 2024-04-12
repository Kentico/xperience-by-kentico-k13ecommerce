using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using Kentico.Xperience.StoreApi.Currencies;

namespace Kentico.Xperience.StoreApi.Products;

public class ProductPageRequest
{
    [Required]
    public string Path { get; set; }
    [RegularExpression("[a-zA-Z]{2}-[a-zA-Z]{2}")]
    public string Culture { get; set; }
    [CurrencyValidation]
    public string Currency { get; set; }
    public string OrderBy { get; set; }
    [DefaultValue(12)]
    [Range(1, 1000)]
    public int Limit { get; set; }

    public bool WithVariants { get; set; }

    public bool WithLongDescription { get; set; }
    public bool NoLinks { get; set; }

    public void Deconstruct(out string path, out string culture, out string currency, out string orderBy,
        out int limit, out bool withVariants, out bool withLongDescription, out bool noLinks)
    {
        path = Path;
        culture = Culture;
        currency = Currency;
        orderBy = OrderBy;
        limit = Limit;
        withVariants = WithVariants;
        withLongDescription = WithLongDescription;
        noLinks = NoLinks;
    }
}
