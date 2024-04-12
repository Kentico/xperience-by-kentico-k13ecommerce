namespace Kentico.Xperience.Ecommerce.Common.ContentItemSynchronization;

/// <summary>
/// Parameters for content item creation in synchronization
/// </summary>
public class ContentItemAddParams
{
    public required ContentItemSynchronizationBase ContentItem { get; set; }
    public required string LanguageName { get; set; }
    public int UserID { get; set; }
}
