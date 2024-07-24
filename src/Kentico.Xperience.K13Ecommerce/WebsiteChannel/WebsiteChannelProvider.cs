using CMS.ContentEngine;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.Websites;

namespace Kentico.Xperience.K13Ecommerce.WebsiteChannel
{
    internal class WebsiteChannelProvider : IWebsiteChannelProvider
    {
        private readonly IInfoProvider<WebsiteChannelInfo> websiteChannelInfoProvider;
        private readonly IProgressiveCache progressiveCache;

        public WebsiteChannelProvider(IInfoProvider<WebsiteChannelInfo> websiteChannelInfoProvider, IProgressiveCache progressiveCache)
        {
            this.websiteChannelInfoProvider = websiteChannelInfoProvider;
            this.progressiveCache = progressiveCache;
        }

        public async Task<int> GetWebsiteChannelId(string channelName, CancellationToken cancellationToken = default)
        {
            var websiteChannel = await progressiveCache.LoadAsync(
                async cacheSettings =>
                {
                    var result = await websiteChannelInfoProvider.Get()
                        .Source(websiteChannelSource => websiteChannelSource
                            .Join<ChannelInfo>(nameof(WebsiteChannelInfo.WebsiteChannelChannelID), nameof(ChannelInfo.ChannelID))
                        )
                        .WhereEquals(nameof(ChannelInfo.ChannelName), channelName)
                        .GetEnumerableTypedResultAsync(cancellationToken: cancellationToken);

                    cacheSettings.CacheDependency = CacheHelper.GetCacheDependency($"{WebsiteChannelInfo.OBJECT_TYPE}|byname|{channelName}");
                    return result.FirstOrDefault();
                },
                new CacheSettings(TimeSpan.FromHours(1).TotalMinutes, $"Channelid|{channelName}")
            );

            if (websiteChannel == null)
            {
                return 0;
            }
            return websiteChannel.WebsiteChannelID;
        }
    }
}
