using System.Net.Http.Headers;

using Kentico.Xperience.K13Ecommerce.Config;

using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace DancingGoat.HealthChecks
{
    internal class K13StoreApiHealthCheck : IHealthCheck
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ILogger<K13StoreApiHealthCheck> logger;
        private readonly IOptions<KenticoStoreConfig> kenticoStoreConfig;

        public K13StoreApiHealthCheck(IHttpClientFactory httpClientFactory, ILogger<K13StoreApiHealthCheck> logger, IOptions<KenticoStoreConfig> kenticoStoreConfig)
        {
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;
            this.kenticoStoreConfig = kenticoStoreConfig;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
        {
            try
            {
                using var httpClient = httpClientFactory.CreateClient(nameof(K13StoreApiHealthCheck));
                var request = new StringContent($"user_email=&grant_type=client_credentials&client_id={kenticoStoreConfig.Value.ClientId}&client_secret={kenticoStoreConfig.Value.ClientSecret}");
                request.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

                var response = await httpClient.PostAsync("/api/store/auth/token", request, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    return HealthCheckResult.Healthy("App is healthy.");
                }
                else
                {
                    var statusCode = response.StatusCode;
                    string reasonPhrase = response.ReasonPhrase;

                    string responseBody = string.Empty;
                    if (response.Content != null)
                    {
                        responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
                    }

                    string errorMessage = $"Request to K13 Store API failed with status code {(int)statusCode} ({statusCode}). " +
                        $"Reason: {reasonPhrase}. " +
                        $"Response: {responseBody}";
                    logger.LogError(errorMessage);
                    return HealthCheckResult.Unhealthy(errorMessage);
                }
            }
            catch (HttpRequestException e)
            {
                logger.LogError(e, e.Message);
                return HealthCheckResult.Unhealthy(exception: e);
            }
        }
    }
}
