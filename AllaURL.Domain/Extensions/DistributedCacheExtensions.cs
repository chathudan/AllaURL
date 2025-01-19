﻿using AllaURL.Data.Entities;
using AllaURL.Domain.Models;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AllaURL.Domain.Extensions
{
    public static class DistributedCacheExtensions
    {
        public static async Task<TokenEntity> GetDataAsync(this IDistributedCache cache, string token)
        {
            var cachedLinkBytes = await cache.GetAsync(token);
            if (cachedLinkBytes is null || cachedLinkBytes.Length is 0) return null;

            var cachedLinkJson = Encoding.UTF8.GetString((byte[])cachedLinkBytes);
            var cachedLink = JsonSerializer.Deserialize<Token>(cachedLinkJson);

            return cachedLink.ConvertToEntity();
        }

        public static async Task SetDataAsync(this IDistributedCache cache, string token, Token link,
            TimeSpan? ttl = null)
        {
            var json = JsonSerializer.Serialize(link);
            var bytes = Encoding.UTF8.GetBytes((string)json);

            if (ttl == null)
            {
                await cache.SetAsync(token, bytes);
            }
            else
            {
                await cache.SetAsync(token, bytes, new() { SlidingExpiration = ttl });
            }
        }
    }
}
