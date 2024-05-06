using Kentico.Forms.Web.Mvc;

namespace Kentico.Xperience.K13Ecommerce.Components.FormComponents.KenticoStorePropertySelector;

/// <summary>
/// Selector properties for KStore selector.
/// </summary>
public class KenticoStorePropertySelectorProperties : SelectorProperties
{
    /// <summary>
    /// Mode of the selector.
    /// </summary>
    public KenticoStorePropertySelectorMode Mode { get; set; }
}


/// <summary>
/// Enum to specify the mode of selector component, f.e. <see cref="KenticoStorePropertySelectorComponent"/>.
/// </summary>
public enum KenticoStorePropertySelectorMode
{
    /// <summary>
    /// Selector enum for categories.
    /// </summary>
    Category,

    /// <summary>
    /// Selector enum for currencies.
    /// </summary>
    Currency,

    /// <summary>
    /// Selector enum for cultures.
    /// </summary>
    Culture
}
