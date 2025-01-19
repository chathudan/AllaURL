//using System.Text;
//using System.Text.Json;
//using AllaURL.Data.Entities;
//using Microsoft.Extensions.Caching.Distributed;

//namespace AllaURL.Data.Extensions
//{
//    public static class DistributedCacheExtensions
//    {
//        public static async Task<Token> GetDataAsync(this IDistributedCache cache, string token)
//        {
//            var cachedLinkBytes = await cache.GetAsync(token);
//            if (cachedLinkBytes is null || cachedLinkBytes.Length is 0) return null;

//            var cachedLinkJson = Encoding.UTF8.GetString((byte[])cachedLinkBytes);
//            var cachedLink = JsonSerializer.Deserialize<TokenEntity>(cachedLinkJson);

//            return cachedLink;
//        }

//        public static async Task SetDataAsync(this IDistributedCache cache, string token, TokenEntity link,
//            TimeSpan? ttl = null)
//        {
//            var json = JsonSerializer.Serialize(link);
//            var bytes = Encoding.UTF8.GetBytes((string)json);

//            if (ttl == null)
//            {
//                await cache.SetAsync(token, bytes);
//            }
//            else
//            {
//                await cache.SetAsync(token, bytes, new() { SlidingExpiration = ttl });
//            }
//        }
//    }
//}