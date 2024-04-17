using System.Net.Http.Headers;

using Kentico.Xperience.K13Ecommerce.StoreApi;

namespace Kentico.Xperience.KStore.Tests;

public class StoreApiTestBase
{
    protected HttpClient HttpClient = null!;
    protected IKenticoStoreApiClient StoreApiClient = null!;


    [OneTimeSetUp]
    public async Task Setup()
    {
        HttpClient = new HttpClient { BaseAddress = new Uri("http://dev.dancinggoat.com:65375") };

        StoreApiClient = new KenticoStoreApiClient(HttpClient);

        var tokenResponse = await StoreApiClient.TokenAsync("client_credentials",
            client_id: "3ef7fe1b-696c-4afa-8b56-d3176b7bea95",
            client_secret: "dgKQeq0y3E59qCcSICAl", username: "public");

        HttpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", tokenResponse.Access_token);
    }


    [OneTimeTearDown]
    public void TearDown()
    {
        HttpClient?.Dispose();
    }
}
