using System;
using System.Security.Cryptography;
using System.Text;

namespace CEKA_APP.Security
{
    public static class SecurityHelpers
    {
        public static string ProtectString(string plainText)
        {
            if (plainText == null) return null;

            var plainBytes = Encoding.UTF8.GetBytes(plainText);

            byte[] encrypted = ProtectedData.Protect(plainBytes, null, DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(encrypted);
        }

        public static string UnprotectString(string encryptedBase64)
        {
            if (string.IsNullOrEmpty(encryptedBase64)) return null;

            try
            {
                byte[] encrypted = Convert.FromBase64String(encryptedBase64);
                // var entropy = Encoding.UTF8.GetBytes("app-specific-entropy");
                byte[] plainBytes = ProtectedData.Unprotect(encrypted, null, DataProtectionScope.CurrentUser);
                return Encoding.UTF8.GetString(plainBytes);
            }
            catch
            {
                return null;
            }
        }
    }
}
