using Kentico.Xperience.Admin.Base.Forms;

namespace Kentico.Xperience.K13Ecommerce.Admin.Validations;

[ValidationRuleAttribute(typeof(XbKPagePathValidationRuleAttribute))]
internal class XbKPagePathValidationRule : ValidationRule<string>
{
    public override Task<ValidationResult> Validate(string value, IFormFieldValueProvider formFieldValueProvider)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return ValidationResult.SuccessResult();
        }
        if (value.Contains('.'))
        {
            return ValidationResult.FailResult("Value cannot contain dots.");
        }
        if (!formFieldValueProvider.TryGet<string>(nameof(PagePathMappingRuleConfigurationModel.K13NodeAliasPath), out string? k13NodeAliasPath))
        {
            return ValidationResult.FailResult($"Cannot find {K13EcommerceTableConstants.K13NodeAliasPathCaption}");
        }
        if (ValidatePathTokensConsistency(k13NodeAliasPath, value, out string errorMessage))
        {
            return ValidationResult.SuccessResult();
        }
        return ValidationResult.FailResult(errorMessage);
    }

    private static bool ValidatePathTokensConsistency(string k13NodeAliasPath, string xbkPagePath, out string errorMessage)
    {
        string[] k13NodeAliasPathSegments = k13NodeAliasPath.Split('/');
        string[] xbkPagePathSegments = xbkPagePath.Split('/');

        var k13NodeAliasPathTokens = VariableTokens.ExtractTokens(k13NodeAliasPathSegments);

        string? inconsistentVariableToken = Array.Find(
            xbkPagePathSegments,
            segment => VariableTokens.IsVariableToken(segment) && !k13NodeAliasPathTokens.Contains(segment)
        );

        if (inconsistentVariableToken is not null)
        {
            errorMessage = $"Variable token '{inconsistentVariableToken}' from {K13EcommerceTableConstants.XbKPagePathCaption} must be present in {K13EcommerceTableConstants.K13NodeAliasPathCaption}";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }
}
