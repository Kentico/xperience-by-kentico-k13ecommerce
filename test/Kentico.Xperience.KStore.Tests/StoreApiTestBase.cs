using System.Net.Http.Headers;

using Kentico.Xperience.K13Ecommerce.StoreApi;

namespace Kentico.Xperience.KStore.Tests;

public class StoreApiTestBase
{
    protected HttpClient httpClient = null!;
    protected IKenticoStoreApiClient storeApiClient = null!;

    [OneTimeSetUp]
    public async Task Setup()
    {
        httpClient = new HttpClient { BaseAddress = new Uri("http://dev.dancinggoat.com:65375") };

        storeApiClient = new KenticoStoreApiClient(httpClient);

        var tokenResponse = await storeApiClient.TokenAsync("client_credentials",
            client_id: "3ef7fe1b-696c-4afa-8b56-d3176b7bea95",
            client_secret: "dgKQeq0y3E59qCcSICAl", username: "public");

        httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", tokenResponse.Access_token);
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        httpClient?.Dispose();
    }
}
