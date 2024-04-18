using Kentico.Xperience.Ecommerce.Common.ContentItemSynchronization.Interfaces;

namespace Kentico.Xperience.K13Ecommerce.Synchronization.ProductImages;

/// <summary>
/// Product image DTO
/// </summary>
public class ProductImageDto : IItemIdentifier<string>
{
    public required string ImageUrl { get; set; }


    public required string ImageDescription { get; set; }


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
