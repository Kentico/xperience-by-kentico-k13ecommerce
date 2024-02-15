using Kentico.Forms.Web.Mvc;

namespace Kentico.Xperience.K13Ecommerce.Components.FormComponents.KenticoStorePropertySelector;
public class KenticoStorePropertySelectorProperties : SelectorProperties
{
    public KenticoStorePropertySelectorMode Mode { get; set; }
}

public enum KenticoStorePropertySelectorMode
{
    Category,
    Currency,
    Culture
}
