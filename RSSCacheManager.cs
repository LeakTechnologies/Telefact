using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Caching;
using System.ServiceModel.Syndication;

namespace Telefact
{
    public class RSSCacheManager
    {
        private readonly MemoryCache _cache = MemoryCache.Default;
        private readonly RSSFeedParser _parser = new RSSFeedParser();
        private const double CacheMinutes = 15;

        public SyndicationFeed GetFeed(string category)
        {
            string key = $"Feed_{category}";
            if (_cache.Contains(key))
            {
                Debug.WriteLine($"[RSSCacheManager] Returning CACHED feed for '{category}'");
                return (SyndicationFeed)_cache.Get(key);
            }

            if (!_parser.Feeds.TryGetValue(category, out var url))
            {
                Debug.WriteLine($"[RSSCacheManager] No URL configured for '{category}'");
                return null;
            }

            Debug.WriteLine($"[RSSCacheManager] Fetching feed for '{category}' from {url}");
            var feed = _parser.GetFeed(url);
            if (feed != null)
            {
                _cache.Set(key, feed, DateTimeOffset.Now.AddMinutes(CacheMinutes));
                Debug.WriteLine($"[RSSCacheManager] Fetched and CACHED feed for '{category}' ({feed.Items.Count()} items)");
            }
            else
            {
                Debug.WriteLine($"[RSSCacheManager] FAILED to fetch feed for '{category}'");
            }
            return feed;
        }
    }
}
