using AutoMapper;
using CMS.Ecommerce;
using Kentico.Xperience.StoreApi.Currencies;
using Kentico.Xperience.StoreApi.Products.Prices;
using Kentico.Xperience.StoreApi.Products.SKU;

namespace Kentico.Xperience.StoreApi;

public class StoreApiMappingProfile : Profile
{
    public StoreApiMappingProfile()
    {
        CreateMap<CurrencyInfo, KCurrency>();
        CreateMap<ProductCatalogPrices, KProductCatalogPrices>();
        CreateMap<SKUInfo, KProductVariant>();
    }
}