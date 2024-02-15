using System.ComponentModel.DataAnnotations;
using CMS.Ecommerce;
using CMS.SiteProvider;

namespace Kentico.Xperience.StoreApi.Currencies;
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
internal class CurrencyValidationAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is null)
        {
            return ValidationResult.Success;
        }

        if (value is not string currency || string.IsNullOrWhiteSpace(currency))
        {
            return new ValidationResult("Currency needs to be string with currencyCode");
        }

        bool isValid = CurrencyInfoProvider.GetCurrencies(SiteContext.CurrentSiteID, onlyEnabled: true)
            .Column(nameof(CurrencyInfo.CurrencyCode))
            .Select(c => c.CurrencyCode)
            .Contains(currency);

        return isValid ?
            ValidationResult.Success :
            new ValidationResult($"CurrencyCode '{currency}' is not valid for site");
    }
}
