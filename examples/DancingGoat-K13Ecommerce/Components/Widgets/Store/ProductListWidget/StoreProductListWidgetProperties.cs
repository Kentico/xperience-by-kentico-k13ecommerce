using Kentico.Components.Web.Mvc.FormComponents;
using Kentico.Forms.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Xperience.K13Ecommerce.Components.FormComponents.KenticoStorePropertySelector;
using System.ComponentModel.DataAnnotations;

namespace DancingGoat.Components.Widgets.Store.ProductListWidget;

public class StoreProductListWidgetProperties : IWidgetProperties
{
    [EditingComponent(TextInputComponent.IDENTIFIER, Label = "Title", Order = 0)]
    public string Title { get; set; }

    [EditingComponent(KenticoStorePropertySelectorComponent.IDENTIFIER, Label = "Section", Order = 10)]
    [EditingComponentProperty(nameof(KenticoStorePropertySelectorProperties.Mode), KenticoStorePropertySelectorMode.Category)]
    public string Path { get; set; }
    
    [EditingComponent(KenticoStorePropertySelectorComponent.IDENTIFIER, Label = "Culture", Order = 20)]
    [EditingComponentProperty(nameof(KenticoStorePropertySelectorProperties.Mode), KenticoStorePropertySelectorMode.Culture)]
    public string Culture { get; set; }

    [EditingComponent(KenticoStorePropertySelectorComponent.IDENTIFIER, Label = "Currency", Order = 30)]
    [EditingComponentProperty(nameof(KenticoStorePropertySelectorProperties.Mode), KenticoStorePropertySelectorMode.Currency)]
    public string CurrencyCode { get; set; }

    [EditingComponent(TextInputComponent.IDENTIFIER, Label = "Order By", Order = 40)]
    public string OrderBy { get; set; }

    [EditingComponent(IntInputComponent.IDENTIFIER, Label = "Max results", Order = 50)]
    [Range(1, 250)]
    public int Limit { get; set; }
}
