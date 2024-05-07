namespace Kentico.Xperience.StoreApi.Currencies;

/// <summary>
/// Represents currency from Store configuration. <see cref="CMS.Ecommerce.CurrencyInfo"/>
/// </summary>
public class KCurrency
{
    public int CurrencyId { get; set; }
    public string CurrencyCode { get; set; }
    public string CurrencyFormatString { get; set; }
}
