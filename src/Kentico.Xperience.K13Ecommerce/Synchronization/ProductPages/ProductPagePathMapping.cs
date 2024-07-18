using System.Text.RegularExpressions;

using CMS.Integration.K13Ecommerce;

namespace Kentico.Xperience.K13Ecommerce.Synchronization.ProductPages
{
    internal static partial class ProductPagePathMapping
    {
        public static ProductPagePathModel? MapPath(string nodeAliasPath, IList<PagePathMappingRuleInfo> rules)
        {
            foreach (var rule in rules)
            {
                string pattern = rule.PagePathMappingRuleK13NodeAliasPath;
                string replacement = rule.PagePathMappingRuleXbKPagePath;

                if (TransformPath(nodeAliasPath, pattern, replacement, out string transformedPath))
                {
                    return new ProductPagePathModel(transformedPath, rule.PagePathMappingRuleChannelName);
                }
            }
            return null;
        }

        private static bool TransformPath(string path, string pattern, string replacement, out string transformedPath)
        {
            // Extract token names from the user pattern.
            var tokenNames = new List<string>();
            // Either {Variable} or three dots.
            var tokenRegex = TokenRegexCompiled();
            var matches = tokenRegex.Matches(pattern);

            // Scan for variables or three dots.
            for (int i = 0; i < matches.Count; i++)
            {
                tokenNames.Add(matches[i].Value);
            }

            // Convert user-friendly pattern to regex pattern
            string regexPattern = pattern;
            for (int i = 0; i < tokenNames.Count; i++)
            {
                regexPattern = i == tokenNames.Count - 1
                    ? regexPattern.Replace(tokenNames[i], $"(.+)")
                    : regexPattern.Replace(tokenNames[i], $"([^/]+)");
            }
            regexPattern = regexPattern.Replace("...", "(.+)");
            regexPattern = "^" + regexPattern + "$";

            // Convert user-friendly replacement to regex replacement
            string regexReplacement = replacement;
            for (int i = 0; i < tokenNames.Count; i++)
            {
                regexReplacement = regexReplacement.Replace(tokenNames[i], $"${i + 1}");
            }

            if (Regex.Match(path, regexPattern).Success)
            {
                transformedPath = Regex.Replace(path, regexPattern, regexReplacement);
                return true;
            }

            transformedPath = string.Empty;
            return false;
        }

        [GeneratedRegex(@"(\{[^}]+\})|(\.\.\.)")]
        private static partial Regex TokenRegexCompiled();
    }
}
