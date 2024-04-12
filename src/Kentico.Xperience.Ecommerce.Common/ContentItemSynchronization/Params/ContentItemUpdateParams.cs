using CMS.ContentEngine;

namespace Kentico.Xperience.Ecommerce.Common.ContentItemSynchronization;

/// <summary>
/// Parameters for content item update in synchronization
/// </summary>
public class ContentItemUpdateParams
{
    public required Dictionary<string, object?> ContentItemParams { get; set; }
    public required int ContentItemID { get; set; }
    public required string LanguageName { get; set; }
    public required int UserID { get; set; }
    public required VersionStatus VersionStatus { get; set; }
}
