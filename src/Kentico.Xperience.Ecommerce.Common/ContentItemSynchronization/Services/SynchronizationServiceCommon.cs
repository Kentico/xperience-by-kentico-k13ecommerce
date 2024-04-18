using System.Net.Mime;

using CMS.ContentEngine;
using CMS.Core;

using Kentico.Xperience.Ecommerce.Common.ContentItemSynchronization.Interfaces;

using Path = CMS.IO.Path;

namespace Kentico.Xperience.Ecommerce.Common.ContentItemSynchronization;

public abstract class SynchronizationServiceCommon(IHttpClientFactory httpClientFactory)
{
    protected (IEnumerable<TStoreItem> ToCreate, IEnumerable<(TStoreItem StoreItem, TContentItem ContentItem)> ToUpdate,
        IEnumerable<TContentItem> ToDelete)
        ClassifyItems<TStoreItem, TContentItem, TType>(IEnumerable<TStoreItem> storeItems,
            IEnumerable<TContentItem> existingItems)
        where TStoreItem : IItemIdentifier<TType>
        where TContentItem : IItemIdentifier<TType>
    {
        var existingLookup = existingItems.ToLookup(item => item.ExternalId);
        var storeLookup = storeItems.ToLookup(item => item.ExternalId);

        var toCreate = storeItems.Where(storeItem => !existingLookup.Contains(storeItem.ExternalId))
            .ToList();

        var toUpdate = storeItems.SelectMany(storeItem => existingLookup[storeItem.ExternalId],
                (storeItem, existingItem) => (storeItem, existingItem))
            .ToList();

        var toDelete = existingItems.Where(p => !storeLookup.Contains(p.ExternalId)).ToList();

        return (toCreate, toUpdate, toDelete);
    }

    protected async Task<ContentItemAssetMetadataWithSource> CreateAssetMetadata(string url, string name)
    {
        byte[] bytes;
        string? contentType;
        using (var client = httpClientFactory.CreateClient())
        {
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            bytes = await response.Content.ReadAsByteArrayAsync();
            contentType = response.Content.Headers.ContentType?.MediaType;
        }

        long length = bytes.LongLength;
        var dataWrapper = new BinaryDataWrapper(bytes);
        var fileSource = new ContentItemAssetStreamSource(cancellationToken => Task.FromResult(dataWrapper.Stream));
        string extension = Path.GetExtension(name);
        if (string.IsNullOrWhiteSpace(extension))
        {
            extension = GetExtension(contentType);
            name += extension;
        }

        var assetMetadata = new ContentItemAssetMetadata()
        {
            Extension = extension,
            Identifier = Guid.NewGuid(),
            LastModified = DateTime.Now,
            Name = name,
            Size = length
        };

        return new ContentItemAssetMetadataWithSource(fileSource, assetMetadata);
    }

    private static string GetExtension(string? contentType) =>
        contentType switch
        {
            MediaTypeNames.Image.Jpeg => ".jpg",
            MediaTypeNames.Image.Png => ".png",
            MediaTypeNames.Image.Webp => ".webp",
            MediaTypeNames.Image.Avif => ".avif",
            _ => string.Empty
        };
}
