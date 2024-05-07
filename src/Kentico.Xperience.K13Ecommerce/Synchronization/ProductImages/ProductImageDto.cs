using Kentico.Xperience.Ecommerce.Common.ContentItemSynchronization;

namespace Kentico.Xperience.K13Ecommerce.Synchronization.ProductImages;

/// <summary>
/// Product image DTO.
/// </summary>
public class ProductImageDto : IItemIdentifier<string>
{
    /// <summary>
    /// Image URL.
    /// </summary>
    public required string ImageUrl { get; set; }


    /// <summary>
    /// Image description.
    /// </summary>
    public required string ImageDescription { get; set; }


    /// <inheritdoc/>
    public string ExternalId
    {
        get
        {
            try
            {
                return new Uri(ImageUrl).PathAndQuery;
            }
            catch (UriFormatException)
            {
                return string.Empty;
            }
        }
    }
}
