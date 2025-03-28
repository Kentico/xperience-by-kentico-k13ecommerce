using Kentico.Xperience.Ecommerce.Common.ContentItemSynchronization;

namespace Kentico.Xperience.K13Ecommerce.Synchronization;

/// <summary>
/// Common service base functionality for synchronization of content items.
/// </summary>
public static class SynchronizationHelper
{
    /// <summary>
    /// Classify items for synchronization to create, update and delete group according to current state.
    /// </summary>
    /// <typeparam name="TStoreItem"></typeparam>
    /// <typeparam name="TContentItem"></typeparam>
    /// <typeparam name="TType"></typeparam>
    /// <param name="storeItems"></param>
    /// <param name="existingItems"></param>
    /// <returns></returns>
    public static (IEnumerable<TStoreItem> ToCreate, IEnumerable<(TStoreItem StoreItem, TContentItem ContentItem)> ToUpdate,
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
}
