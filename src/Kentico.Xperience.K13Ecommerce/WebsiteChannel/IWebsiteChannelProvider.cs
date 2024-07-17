namespace Kentico.Xperience.K13Ecommerce.WebsiteChannel
{
    public interface IWebsiteChannelProvider
    {
        Task<int> GetWebsiteChannelId(string channelName, CancellationToken cancellationToken = default);
    }
}
