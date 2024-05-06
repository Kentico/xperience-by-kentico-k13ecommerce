using Kentico.Forms.Web.Mvc;
using Kentico.Web.Mvc;
using Kentico.Xperience.K13Ecommerce.Components.FormComponents.KenticoStorePropertySelector;
using Kentico.Xperience.K13Ecommerce.StoreApi;

[assembly: RegisterFormComponent(KenticoStorePropertySelectorComponent.IDENTIFIER, typeof(KenticoStorePropertySelectorComponent), "Kentico Store properties selector", IconClass = "icon-menu")]

namespace Kentico.Xperience.K13Ecommerce.Components.FormComponents.KenticoStorePropertySelector;

/// <summary>
/// KStore selector component for currencies,cultures and categories.
/// </summary>
/// <param name="storeApiClient">Store API client.</param>
public class KenticoStorePropertySelectorComponent(IKenticoStoreApiClient storeApiClient) : SelectorFormComponent<KenticoStorePropertySelectorProperties>
{
    /// <summary>
    /// Store property selector identifier.
    /// </summary>
    public const string IDENTIFIER = "Kentico.Xperience.Store.PropertySelector";


    /// <inheritdoc/>
    protected override IEnumerable<HtmlOptionItem> GetHtmlOptions()
    {
        if (Properties.Mode == KenticoStorePropertySelectorMode.Category)
        {
            var categories = storeApiClient.GetProductCategoriesAsync().GetAwaiter().GetResult();

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
            var currencies = storeApiClient.GetCulturesAsync().GetAwaiter().GetResult();

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
            var currencies = storeApiClient.GetCurrenciesAsync().GetAwaiter().GetResult();

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
