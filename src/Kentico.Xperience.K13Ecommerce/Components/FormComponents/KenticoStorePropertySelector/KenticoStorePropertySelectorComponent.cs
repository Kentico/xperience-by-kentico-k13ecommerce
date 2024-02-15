using Kentico.Forms.Web.Mvc;
using Kentico.Web.Mvc;
using Kentico.Xperience.K13Ecommerce.Components.FormComponents.KenticoStorePropertySelector;
using Kentico.Xperience.K13Ecommerce.KenticoStoreApi;

[assembly: RegisterFormComponent(KenticoStorePropertySelectorComponent.IDENTIFIER, typeof(KenticoStorePropertySelectorComponent), "Kentico Store properties selector", IconClass = "icon-menu")]
namespace Kentico.Xperience.K13Ecommerce.Components.FormComponents.KenticoStorePropertySelector;

public class KenticoStorePropertySelectorComponent(IKStoreApiService storeApiService) : SelectorFormComponent<KenticoStorePropertySelectorProperties>
{
    public const string IDENTIFIER = "Kentico.Xperience.Store.PropertySelector";

    // Retrieves data to be displayed in the selector
    protected override IEnumerable<HtmlOptionItem> GetHtmlOptions()
    {
        if (Properties.Mode == KenticoStorePropertySelectorMode.Category)
        {
            var categories = storeApiService.GetProductCategories().GetAwaiter().GetResult();

            yield return new HtmlOptionItem()
            {
                Value = "/",
                Text = "All"
            };

            foreach (var c in categories)
            {
                yield return new HtmlOptionItem()
                {
                    Value = c.Path,
                    Text = $"{c.Name} ({c.Path})"
                };
            }
        }
        else if (Properties.Mode == KenticoStorePropertySelectorMode.Culture)
        {
            var currencies = storeApiService.GetCultures().GetAwaiter().GetResult();

            foreach (var c in currencies)
            {
                yield return new HtmlOptionItem()
                {
                    Value = c.CultureCode,
                    Text = c.CultureName
                };
            }
        }
        else if (Properties.Mode == KenticoStorePropertySelectorMode.Currency)
        {
            var currencies = storeApiService.GetCurrencies().GetAwaiter().GetResult();

            foreach (string c in currencies)
            {
                yield return new HtmlOptionItem()
                {
                    Value = c,
                    Text = c
                };
            }
        }
    }
}
