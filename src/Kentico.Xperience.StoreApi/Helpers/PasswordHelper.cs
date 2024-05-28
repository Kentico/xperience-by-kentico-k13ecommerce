using System.Security.Cryptography;

namespace Kentico.Xperience.StoreApi.Helpers;

internal static class PasswordHelper
{
    /// <summary>
    /// Generates a random password of the specified length.
    /// </summary>
    /// <param name="length">Required password length</param>
    /// <returns>Generated password</returns>
    public static string GeneratePassword(int length)
    {
        var rng = RandomNumberGenerator.Create();
        byte[] bytes = new byte[length];
        rng.GetBytes(bytes);
        return Convert.ToBase64String(bytes)[..length];
    }
}
