using AutoMapper;
using CMS.DocumentEngine;
using CMS.Ecommerce;
using Kentico.Xperience.StoreApi.Products.SKU;

namespace Kentico.Xperience.StoreApi.Products.Pages;

/// <summary>
/// Implemented converter from Kentico product page to product page model
/// </summary>
/// <typeparam name="TProduct"></typeparam>
public class ProductPageConverter<TProduct> : IProductPageConverter<TProduct>
    where TProduct : KProductNode, new()
{
    private readonly IProductSKUConverter<KProductSKU> skuConverter;
    private readonly IMapper mapper;


    public ProductPageConverter(IProductSKUConverter<KProductSKU> skuConverter)
    {
        this.skuConverter = skuConverter;
        mapper = new MapperConfiguration(cfg =>
                cfg.CreateMap<SKUTreeNode, TProduct>().ForMember(p => p.SKU, o => o.Ignore()))
            .CreateMapper();
    }

    public virtual TProduct Convert(SKUTreeNode skuTreeNode, IEnumerable<string> customFields, string currencyCode)
    {
        var model = mapper.Map<TProduct>(skuTreeNode);
        model.AbsoluteUrl = DocumentURLProvider.GetAbsoluteUrl(skuTreeNode);
        model.SKU = skuConverter.Convert(skuTreeNode.SKU, currencyCode);
        model.CustomFields = customFields.ToDictionary(f => f, f => skuTreeNode.GetValue(f));
        return model;
    }
}