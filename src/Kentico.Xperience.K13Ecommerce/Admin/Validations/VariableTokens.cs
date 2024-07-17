namespace Kentico.Xperience.K13Ecommerce.Admin.Validations
{
    internal static class VariableTokens
    {
        public static HashSet<string> ExtractTokens(string[] segments)
            => segments.Where(IsVariableToken).ToHashSet();

        public static bool IsVariableToken(string? segment)
        {
            if (string.IsNullOrEmpty(segment))
            {
                return false;
            }
            return segment.StartsWith('{') && segment.EndsWith('}') && segment.Length > 2;
        }
    }
}
