using AutoMapper;

using CMS.Helpers;

namespace Kentico.Xperience.StoreApi.Mapping;

internal class ContainerCustomDataToDictionaryConverter : ITypeConverter<ContainerCustomData, Dictionary<string, object>>
{
    public Dictionary<string, object> Convert(ContainerCustomData source, Dictionary<string, object> destination,
        ResolutionContext context) =>
        source.ColumnNames.Select(c => (Key: c, Value: source[c]))
            .ToDictionary(c => c.Key, c => c.Value);
}
