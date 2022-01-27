using System;

namespace MockItUp.Common.Utilities
{
    public static class IdGen
    {
        public static string Generate()
        {
            return Encode(Guid.NewGuid());
        }

        /// <summary>
        /// Encodes the given <see cref="System.Guid"/> as an encoded ShortGuid string. The encoding is
        /// similar to Base64, with some non-URL safe characters replaced, and padding removed, resulting
        /// in a 22 character string.
        /// </summary>
        /// <param name="guid">The <see cref="System.Guid"/> to encode.</param>
        /// <returns>A 22 character ShortGuid URL-safe Base64 string.</returns>
        private static string Encode(Guid guid)
        {
            var encoded = Convert.ToBase64String(guid.ToByteArray());

            encoded = encoded
                .Replace("/", "_")
                .Replace("+", "-");

            return encoded.Substring(0, 22);
        }
    }
}
