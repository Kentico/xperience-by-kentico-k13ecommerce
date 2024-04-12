using Duende.AccessTokenManagement;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Kentico.Xperience.K13Ecommerce.Users;

internal class CustomClientCredentialsTokenEndpointService : ClientCredentialsTokenEndpointService
{
    private readonly IHttpContextAccessor httpContextAccessor;

    public CustomClientCredentialsTokenEndpointService(IHttpClientFactory httpClientFactory,
        IOptionsMonitor<ClientCredentialsClient> options, IClientAssertionService clientAssertionService,
        IDPoPKeyStore dPoPKeyMaterialService, IDPoPProofService dPoPProofService,
        ILogger<ClientCredentialsTokenEndpointService> logger, IHttpContextAccessor httpContextAccessor) :
        base(httpClientFactory, options, clientAssertionService, dPoPKeyMaterialService, dPoPProofService, logger)
        => this.httpContextAccessor = httpContextAccessor;


    public override async Task<ClientCredentialsToken> RequestToken(string clientName,
        TokenRequestParameters? parameters = null,
        CancellationToken cancellationToken = new CancellationToken())
    {
        parameters ??= new TokenRequestParameters();

        string userName = httpContextAccessor.HttpContext?.User.Identity?.Name ?? "public";

        parameters.Parameters.Add("username", userName);
        return await base.RequestToken(clientName, parameters, cancellationToken);
    }
}
