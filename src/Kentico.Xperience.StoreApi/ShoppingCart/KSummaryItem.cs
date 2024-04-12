namespace Kentico.Xperience.StoreApi.ShoppingCart;

/// <summary>
/// Dto for <see cref="CMS.Ecommerce.SummaryItem"/>
/// </summary>
public class KSummaryItem
{
    /// <summary>
    /// Summary item name used for displaying in Invoice, Email template and Shopping cart.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Summary item value in shopping cart currency used for displaying in Invoice, Email template and Shopping cart.
    /// </summary>
    public decimal Value { get; set; }
}
