using CMS.Membership;

using Duende.AccessTokenManagement;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Kentico.Xperience.K13Ecommerce.Users;

internal class CustomClientCredentialsTokenEndpointService : ClientCredentialsTokenEndpointService
{
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly IMemberInfoProvider memberInfoProvider;

    public CustomClientCredentialsTokenEndpointService(IHttpClientFactory httpClientFactory,
        IOptionsMonitor<ClientCredentialsClient> options, IClientAssertionService clientAssertionService,
        IDPoPKeyStore dPoPKeyMaterialService, IDPoPProofService dPoPProofService,
        ILogger<ClientCredentialsTokenEndpointService> logger, IHttpContextAccessor httpContextAccessor,
        IMemberInfoProvider memberInfoProvider) :
        base(httpClientFactory, options, clientAssertionService, dPoPKeyMaterialService, dPoPProofService, logger)
    {
        this.httpContextAccessor = httpContextAccessor;
        this.memberInfoProvider = memberInfoProvider;
    }

    public override async Task<ClientCredentialsToken> RequestToken(string clientName,
        TokenRequestParameters? parameters = null,
        CancellationToken cancellationToken = new CancellationToken())
    {
        parameters ??= new TokenRequestParameters();

        string userName = httpContextAccessor.HttpContext?.User.Identity?.Name ?? string.Empty;
        string userEmail = userName != string.Empty ? (await memberInfoProvider.Get()
            .TopN(1)
            .WhereEquals(nameof(MemberInfo.MemberName), userName)
            .GetEnumerableTypedResultAsync()).FirstOrDefault()?.MemberEmail ?? string.Empty : string.Empty;

        parameters.Parameters.Add("user_email", userEmail);
        return await base.RequestToken(clientName, parameters, cancellationToken);
    }
}
