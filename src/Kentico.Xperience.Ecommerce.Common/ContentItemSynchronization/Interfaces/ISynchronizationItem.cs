using CMS.ContentEngine;

namespace Kentico.Xperience.Ecommerce.Common.ContentItemSynchronization.Interfaces;

public interface ISynchronizationItem<TContentItem>
    where TContentItem : IContentItemBase, new()
{
    /// <summary>
    /// Fills modifiedProps with modified properties compared to contentItem
    /// </summary>
    /// <param name="contentItem"></param>
    /// <param name="modifiedProps"></param>
    /// <returns>True if any property was modified. Otherwise False</returns>
    public bool GetModifiedProperties(TContentItem contentItem, out Dictionary<string, object?> modifiedProps);
}


/// <summary>
/// Interface for synchronization between data in Dto and content items
/// </summary>
/// <typeparam name="TDto"></typeparam>
/// <typeparam name="TContentItem"></typeparam>
public interface ISynchronizationItem<TDto, in TContentItem>
    where TDto : class
    where TContentItem : IContentItemFieldsSource
{
    /// <summary>
    /// Dto item, typically object from Ecommerce platform API
    /// </summary>
    public TDto Item { get; set; }


    /// <summary>
    /// Fills modifiedProps with modified properties compared to contentItem
    /// </summary>
    /// <param name="contentItem"></param>
    /// <param name="modifiedProps"></param>
    /// <returns>True if any property was modified. Otherwise False</returns>
    bool GetModifiedProperties(TContentItem contentItem, out Dictionary<string, object?> modifiedProps);
}
