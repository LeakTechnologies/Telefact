using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Caching;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;

namespace Telefact
{
    public class RSSCacheManager
    {
        private readonly MemoryCache _cache = MemoryCache.Default;
        private readonly RSSFeedParser _parser = new RSSFeedParser();
        private const double CacheMinutes = 15;

        /// <summary>
        /// Returns the cached feed for the given category immediately.
        /// If the feed is not yet cached, kicks off a background fetch and returns null.
        /// </summary>
        public SyndicationFeed GetFeed(string category)
        {
            string dataKey = $"Feed_{category}";
            string fetchingKey = $"Fetching_{category}";

            if (_cache.Contains(dataKey))
            {
                Debug.WriteLine($"[RSSCacheManager] Returning CACHED feed for '{category}'");
                return (SyndicationFeed)_cache.Get(dataKey);
            }

            // Only start one background fetch per category at a time.
            if (!_cache.Contains(fetchingKey))
            {
                // Sentinel expires in 1 minute so a failed fetch can be retried.
                _cache.Set(fetchingKey, true, DateTimeOffset.Now.AddMinutes(1));
                Task.Run(() => FetchAndCache(category, dataKey, fetchingKey));
                Debug.WriteLine($"[RSSCacheManager] Started background fetch for '{category}'");
            }
            else
            {
                Debug.WriteLine($"[RSSCacheManager] Fetch already in progress for '{category}'");
            }

            return null;
        }

        private void FetchAndCache(string category, string dataKey, string fetchingKey)
        {
            try
            {
                if (!_parser.Feeds.TryGetValue(category, out var url))
                {
                    Debug.WriteLine($"[RSSCacheManager] No URL configured for '{category}'");
                    return;
                }

                var feed = _parser.GetFeed(url);
                if (feed != null)
                {
                    _cache.Set(dataKey, feed, DateTimeOffset.Now.AddMinutes(CacheMinutes));
                    Debug.WriteLine(
                        $"[RSSCacheManager] Fetched and CACHED '{category}' ({feed.Items.Count()} items)");
                }
                else
                {
                    Debug.WriteLine($"[RSSCacheManager] FAILED to fetch feed for '{category}'");
                }
            }
            finally
            {
                _cache.Remove(fetchingKey);
            }
        }
    }
}
