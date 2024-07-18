namespace Kentico.Xperience.K13Ecommerce.WebsiteChannel;

/// <summary>
/// Website channel provider. Connects webstite channel name with its ID.
/// </summary>
public interface IWebsiteChannelProvider
{
    /// <summary>
    /// Get website channel ID by website channel name
    /// </summary>
    /// <param name="channelName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<int> GetWebsiteChannelId(string channelName, CancellationToken cancellationToken = default);
}
