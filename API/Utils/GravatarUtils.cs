﻿using System.Security.Cryptography;
using System.Text;

namespace ShockLink.API.Utils;

public static class GravatarUtils
{
    public static Uri GetImageUrl(string email)
    {
        Span<byte> tempSpan = stackalloc byte[32];
        SHA256.HashData(Encoding.UTF8.GetBytes(email), tempSpan);
        return new Uri($"https://www.gravatar.com/avatar/{Convert.ToHexString(tempSpan)}?d=https%3A%2F%2Fsea.zlucplayz.com%2Ff%2F897bd3f9b09945cf8c4f%2F%3Fraw%3D1");
    }
    
}