using Kentico.Xperience.Shopify.Config;
using Kentico.Xperience.Shopify.Models;
using Microsoft.Extensions.Options;
using ShopifySharp;
using ShopifySharp.Factories;
using ShopifySharp.Filters;
using ShopifySharp.Lists;

namespace Kentico.Xperience.Shopify.Services.ProductService
{
    public class ShopifyProductService : ShopifyServiceBase, IShopifyProductService
    {
        private readonly IProductService productService;
        private readonly Uri shopifyProductUrlBase;
        private readonly string[] _shopifyFields = ["title", "body_html", "handle", "images", "variants"];
        private const string defaultCurrency = "USD";

        private string ShopifyFields => string.Join(",", _shopifyFields);

        public ShopifyProductService(IOptionsMonitor<ShopifyConfig> options, IProductServiceFactory productServiceFactory) : base(options)
        {
            productService = productServiceFactory.Create(shopifyCredentials);

            var uriBuilder = new UriBuilder(options.CurrentValue.ShopifyUrl);
            uriBuilder.Path = "products";
            shopifyProductUrlBase = uriBuilder.Uri;
        }

        public async Task<ListResultWrapper<ProductListModel>> GetProductsAsync(ProductFilter initialFilter)
        {
            return await TryCatch(async () =>
            {
                return await GetProductsAsyncInternal(initialFilter);
            }, GenerateEmptyResult);
        }

        public async Task<ListResultWrapper<ProductListModel>> GetProductsAsync(PagingFilterParams filterParams)
        {
            return await TryCatch(async () =>
            {
                return await GetProductsAsyncInternal(filterParams);
            }, GenerateEmptyResult);
        }

        private async Task<ListResultWrapper<ProductListModel>> GetProductsAsyncInternal(PagingFilterParams filterParams)
        {
            var filter = new ListFilter<Product>(filterParams?.PageInfo, filterParams?.Limit, ShopifyFields);
            var result = await productService.ListAsync(filter, true);

            // TODO set currency dynamically
            return CreateResultModel(result, "USD");
        }

        private async Task<ListResultWrapper<ProductListModel>> GetProductsAsyncInternal(ProductFilter initialFilter)
        {
            var filter = new ProductListFilter
            {
                Fields = ShopifyFields,
                CollectionId = initialFilter.CollectionID,
                Limit = initialFilter.Limit,
                PresentmentCurrencies = new string[] { initialFilter.Currency?.ToString() ?? "" }
            };
            var result = await productService.ListAsync(filter, true);

            return CreateResultModel(result, initialFilter.Currency.ToString());
        }

        private ListResultWrapper<ProductListModel> CreateResultModel(ListResult<Product> products, string? currency)
        {
            var items = new List<ProductListModel>();
            foreach (var item in products.Items)
            {
                var firstImage = item.Images.FirstOrDefault();
                (var price, var listPrice) = GetPrices(item.Variants, currency ?? defaultCurrency);
                items.Add(new ProductListModel
                {
                    Image = firstImage?.Src,
                    ImageAlt = firstImage?.Alt,
                    Name = item.Title,
                    Description = item.BodyHtml,
                    ShopifyUrl = $"{shopifyProductUrlBase.AbsoluteUri}/{Uri.EscapeDataString(item.Handle)}",
                    Price = price,
                    ListPrice = listPrice
                });
            }

            var nextPage = products.GetNextPageFilter();
            var prevPage = products.GetPreviousPageFilter();

            return new ListResultWrapper<ProductListModel>()
            {
                Items = items,
                PrevPageFilter = new PagingFilterParams()
                {
                    PageInfo = prevPage?.PageInfo,
                    Limit = prevPage?.Limit
                },
                NextPageFilter = new PagingFilterParams()
                {
                    PageInfo = nextPage?.PageInfo,
                    Limit = nextPage?.Limit
                }
            };
        }

        private (decimal? price, decimal? listPrice) GetPrices(IEnumerable<ProductVariant> variants, string? currency)
        {
            if (variants == null || !variants.Any())
            {
                return (null, null);
            }
            if (variants.Count() == 1)
            {
                var onlyVariant = variants.First();
                var currencyPrice = onlyVariant.PresentmentPrices?.FirstOrDefault(x => x.Price.CurrencyCode == currency);

                return currencyPrice is { Price: not null } ?
                    (currencyPrice.Price.Amount, currencyPrice.CompareAtPrice?.Amount) : (null, null);
            }

            decimal? minPrice = null;

            foreach (var variant in variants)
            {
                var currencyPrice = variant.PresentmentPrices?.FirstOrDefault(x => x.Price.CurrencyCode == currency);

                if (currencyPrice?.Price.Amount != null)
                {
                    decimal price = currencyPrice.Price.Amount.Value;

                    if (!minPrice.HasValue || price < minPrice.Value)
                    {
                        minPrice = price;
                    }
                }
            }

            return (minPrice, null);
        }

        private ListResultWrapper<ProductListModel> GenerateEmptyResult()
        {
            return new ListResultWrapper<ProductListModel>()
            {
                Items = Enumerable.Empty<ProductListModel>()
            };
        }
    }
}