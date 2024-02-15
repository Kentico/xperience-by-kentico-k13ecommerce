namespace Kentico.Xperience.StoreApi.Products.SKU;

/// <summary>
/// Model for Kentico product variant
/// </summary>
public class KProductVariant
{
    public string SKUName { get; set; }
    public string SKUNumber { get; set; }
    public decimal SKUPrice { get; set; }
    //@TODO calculate prices with catalog price calculator
}