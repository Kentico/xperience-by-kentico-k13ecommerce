using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace DancingGoat.HealthChecks
{
    internal class K13StoreApiHealthCheck : IHealthCheck
    {
        private readonly IHttpClientFactory httpClientFactory;

        public K13StoreApiHealthCheck(IHttpClientFactory httpClientFactory) => this.httpClientFactory = httpClientFactory;

        public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
        {
            try
            {
                using var httpClient = httpClientFactory.CreateClient(nameof(K13StoreApiHealthCheck));
                var response = await httpClient.GetAsync("/status", cancellationToken);

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
                    return HealthCheckResult.Unhealthy(errorMessage);
                }
            }
            catch (HttpRequestException e)
            {
                return HealthCheckResult.Unhealthy(exception: e);
            }
        }
    }
}
