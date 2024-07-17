using System.Text.RegularExpressions;

using Kentico.Xperience.Admin.Base.Forms;

namespace Kentico.Xperience.K13Ecommerce.Admin.Validations;

[ValidationRuleAttribute(typeof(NodeAliasPathValidationRuleAttribute))]
internal partial class NodeAliasPathValidationRule : ValidationRule<string>
{
    /// <summary>
    /// Allowed URL characters.
    /// </summary>
    private static readonly Regex allowedUrlCharactersRegex = AllowedUrlCharactersRegex();

    public override Task<ValidationResult> Validate(string value, IFormFieldValueProvider formFieldValueProvider)
    {
        if (string.IsNullOrWhiteSpace(value) || ValidatePathStructure(value, out var errorMessage))
        {
            return ValidationResult.SuccessResult();
        }
        return ValidationResult.FailResult(errorMessage);
    }

    private static bool ValidatePathStructure(string path, out string errorMessage)
    {
        if (!path.StartsWith('/'))
        {
            errorMessage = "Pattern must start with forward slash";
            return false;
        }

        // Skip first forward slash
        path = path[1..];
        string[] segments = path.Split('/');

        string? invalidSegment = Array.Find(
            segments,
            segment => !VariableTokens.IsVariableToken(segment) && !allowedUrlCharactersRegex.IsMatch(segment) && segment != "..."
        );
        if (invalidSegment is not null)
        {
            errorMessage = $"Segment '{invalidSegment}' must be either variable token or contain only characters from set [A-Za-z0-9_-~].";
            return false;
        }
        if (segments.Count(segment => segment == "...") > 1)
        {
            errorMessage = "Pattern cannot contain three dots ('...') more than once.";
            return false;
        }

        string? multipleVariableName = segments
            .Where(VariableTokens.IsVariableToken)
            .GroupBy(variable => variable)
            .FirstOrDefault(group => group.Count() > 1)?
            .Key;

        if (multipleVariableName is not null)
        {
            errorMessage = $"In pattern was found duplicated variable '{multipleVariableName}'.";
            return false;
        }

        // Check path does end with token variable
        if (!VariableTokens.IsVariableToken(segments.LastOrDefault()))
        {
            errorMessage = "Pattern must end with variable token - e.g. {Product}.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    [GeneratedRegex(@"^[A-Za-z0-9_\-\.]+$", RegexOptions.Compiled)]
    private static partial Regex AllowedUrlCharactersRegex();
}
