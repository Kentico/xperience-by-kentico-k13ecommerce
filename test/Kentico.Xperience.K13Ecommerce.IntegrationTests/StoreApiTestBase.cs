using System.Net.Http.Headers;

using Kentico.Xperience.K13Ecommerce.StoreApi;

namespace Kentico.Xperience.KStore.Tests;

/// <summary>
/// Base class for Store API integration tests.
/// </summary>
public class StoreApiTestBase
{
    /// <summary>
    /// HttpClient for Store API.
    /// </summary>
    protected HttpClient HttpClient = null!;

    /// <summary>
    /// Store API client.
    /// </summary>
    protected IKenticoStoreApiClient StoreApiClient = null!;


    /// <summary>
    /// Setup for Store API integration tests.
    /// </summary>
    /// <returns></returns>
    [OneTimeSetUp]
    public async Task Setup()
    {
        // Setup address and credentials for Store API to run the tests
        HttpClient = new HttpClient { BaseAddress = new Uri("http://dev.dancinggoat.com:65375") };

        StoreApiClient = new KenticoStoreApiClient(HttpClient);

        //Fill your K13 Store API client credentials
        var tokenResponse = await StoreApiClient.TokenAsync("client_credentials",
            client_id: "",
            client_secret: "",
            userEmail: string.Empty);

        HttpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", tokenResponse.Access_token);
    }


    /// <summary>
    /// Tear down for Store API integration tests.
    /// </summary>
    [OneTimeTearDown]
    public void TearDown()
    {
        HttpClient?.Dispose();
    }
}
