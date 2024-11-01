using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace DancingGoat.HealthChecks
{
    internal class K13StoreApiHealthCheck : IHealthCheck
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ILogger<K13StoreApiHealthCheck> logger;

        public K13StoreApiHealthCheck(IHttpClientFactory httpClientFactory, ILogger<K13StoreApiHealthCheck> logger)
        {
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
        {
            try
            {
                using var httpClient = httpClientFactory.CreateClient(nameof(K13StoreApiHealthCheck));
                var response = await httpClient.PostAsync("/api/store/auth/token", new StringContent("test"));
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
