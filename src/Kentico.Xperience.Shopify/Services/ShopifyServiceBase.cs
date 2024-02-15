using Kentico.Xperience.Shopify.Config;
using Microsoft.Extensions.Options;
using ShopifySharp.Credentials;
using ShopifySharp.Infrastructure;

namespace Kentico.Xperience.Shopify.Services
{
    public abstract class ShopifyServiceBase
    {
        protected readonly ShopifyApiCredentials shopifyCredentials;
        protected ShopifyServiceBase(IOptionsMonitor<ShopifyConfig> options)
        {
            var url = options.CurrentValue.ShopifyUrl;
            var apiToken = options.CurrentValue.ApiToken;
            shopifyCredentials = new ShopifyApiCredentials(url, apiToken);
        }

        /// <summary>
        /// Shopify API returns HTTP 404 response when data does not exist.
        /// In order to return default value, the API call needs to be wrapped
        /// into try catch block.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        protected async Task<T> TryCatch<T>(Func<Task<T>> func, Func<T> defaultValue)
        {
            try
            {
                var result = await func.Invoke();
                return result;
            }
            catch (ShopifyHttpException)
            {
                return defaultValue.Invoke();
            }
        }
    }
}

