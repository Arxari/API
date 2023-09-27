﻿using System.Security.Cryptography;
using System.Text;

namespace OpenShock.ServicesCommon.Utils;

public static class GravatarUtils
{
    public static Uri GetImageUrl(string email)
    {
        Span<byte> tempSpan = stackalloc byte[32];
        SHA256.HashData(Encoding.UTF8.GetBytes(email), tempSpan);
        return new Uri($"https://www.gravatar.com/avatar/{Convert.ToHexString(tempSpan).ToLowerInvariant()}?d=https%3A%2F%2Fshocklink.b-cdn.net%2Fweb%2Fshocklink-logo-only.png");
    }
    
}