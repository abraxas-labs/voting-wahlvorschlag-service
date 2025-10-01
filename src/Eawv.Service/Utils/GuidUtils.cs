// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Security.Cryptography;
using System.Text;

namespace Eawv.Service.Utils;

public static class GuidUtils
{
    private const int GuidLength = 16;

    /// <summary>
    /// MD5 is exactly 16 bytes as the guid is.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Security",
        "CA5351:Do Not Use Broken Cryptographic Algorithms",
        Justification = "MD5 is only used for Guid generation, not for protecting something.")]
    private static readonly MD5 MD5 = MD5.Create();

    /// <summary>
    /// Generates a Guid from a string.
    /// Note: Do not use with user-supplied data. This method is not very secure nor does it conform to any Guid standard.
    /// </summary>
    /// <param name="input">The string to generate a Guid from.</param>
    /// <returns>The generated Guid.</returns>
    public static Guid GuidFromString(string input)
    {
        var id = Encoding.UTF8.GetBytes(input);
        if (id.Length > GuidLength)
        {
            id = MD5.ComputeHash(id);
        }
        else
        {
            // fill with 0s
            Array.Resize(ref id, GuidLength);
        }

        return new Guid(id);
    }
}
