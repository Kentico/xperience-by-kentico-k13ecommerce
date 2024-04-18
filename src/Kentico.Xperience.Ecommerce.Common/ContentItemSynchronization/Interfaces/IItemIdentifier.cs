namespace Kentico.Xperience.Ecommerce.Common.ContentItemSynchronization.Interfaces;

/// <summary>
/// Interface to set external identifier for api objects
/// </summary>
/// <typeparam name="TType"></typeparam>
public interface IItemIdentifier<out TType>
{
    public TType ExternalId { get; }
}
