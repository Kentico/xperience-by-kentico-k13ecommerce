using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Ecommerce;
using CMS.FormEngine;
using Kentico.Content.Web.Mvc;
using Kentico.Xperience.StoreApi.Products.Categories;
using Kentico.Xperience.StoreApi.Products.Pages;

namespace Kentico.Xperience.StoreApi.Products;

internal class KProductService : IKProductService
{
    private readonly IPageRetriever pageRetriever;
    private readonly IProductPageConverter<KProductNode> productPageConverter;

    public KProductService(IPageRetriever pageRetriever, IProductPageConverter<KProductNode> productPageConverter)
    {
        this.pageRetriever = pageRetriever;
        this.productPageConverter = productPageConverter;
    }

    public async Task<IEnumerable<KProductNode>> GetProductPages(string path, string culture,
        string currencyCode, int limit, string orderBy)
    {
        if (string.IsNullOrWhiteSpace(orderBy))
        {
            orderBy = "DocumentSKUName ASC";
        }

        var productTypes = (await DataClassInfoProvider.ProviderObject.Get()
                .WhereTrue(nameof(DataClassInfo.ClassIsProduct))
                .Columns(nameof(DataClassInfo.ClassName), nameof(DataClassInfo.ClassFormDefinition))
                .GetEnumerableTypedResultAsync())
            .Select(p => new
            {
                p.ClassName,
                CustomFields = new FormInfo(p.ClassFormDefinition).ItemsList
                    .OfType<FormFieldInfo>()
                    .Where(i => !i.PrimaryKey)
                    .Select(i => i.Name)
                    .ToList()
            })
            .ToList();

        var productPages = (await pageRetriever.RetrieveMultipleAsync(q =>
            {
                q.Types(productTypes.Select(p => p.ClassName).ToArray())
                    .Path(path, PathTypeEnum.Children)
                    .TopN(limit)
                    .OrderBy(orderBy);
                if (culture is not null)
                {
                    q.Culture(culture);
                }
            }
        )).OfType<SKUTreeNode>();

        return productPages.Select(p => productPageConverter.Convert(p,
                productTypes.First(t => t.ClassName == p.ClassName)
                    .CustomFields, currencyCode))
            .ToList();
    }

    public async Task<IEnumerable<KProductCategory>> GetProductCategories(string culture)
    {
        var categoriesTypes = (await DataClassInfoProvider.ProviderObject.Get()
                .WhereTrue(nameof(DataClassInfo.ClassIsProductSection))
                .Column(nameof(DataClassInfo.ClassName))
                .GetEnumerableTypedResultAsync())
            .Select(p => p.ClassName)
            .ToArray();

        return (await pageRetriever.RetrieveMultipleAsync(q =>
            {
                q.Types(categoriesTypes);
                if (culture is not null)
                {
                    q.Culture(culture);
                }
            }))
            .Select(c => new KProductCategory { Name = c.DocumentName, Path = c.NodeAliasPath });
    }
}
