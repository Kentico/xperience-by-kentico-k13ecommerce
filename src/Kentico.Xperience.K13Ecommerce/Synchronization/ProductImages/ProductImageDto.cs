using Kentico.Xperience.Ecommerce.Common.ContentItemSynchronization.Interfaces;

namespace Kentico.Xperience.K13Ecommerce.Synchronization.ProductImages;

/// <summary>
/// Product image DTO
/// </summary>
public class ProductImageDto : IItemIdentifier<Guid>
{
    public required string ImageUrl { get; set; }


    public required string ImageDescription { get; set; }



    private Guid externalId;
    public Guid ExternalId => externalId == Guid.Empty ? externalId = ExtractGuidFromUrl(ImageUrl) : externalId;


    private static Guid ExtractGuidFromUrl(string url)
    {
        // Split the URL by '/'
        string[] parts = url.Split('/');

        // Loop through the parts to find a valid GUID
        foreach (string part in parts)
        {
            if (Guid.TryParse(part, out var result))
            {
                return result;
            }
        }

        // If no GUID is found, return empty Guid which is ignored later
        return Guid.Empty;
    }
}
