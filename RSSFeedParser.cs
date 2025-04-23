using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Xml;

namespace Telefact
{
    /// <summary>
    /// Parses CBC RSS feeds via SyndicationFeed.
    /// </summary>
    public class RSSFeedParser
    {
        // Map category names to their feed URLs.
        private readonly Dictionary<string, string> _feeds =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "Top Stories",                 "https://www.cbc.ca/webfeed/rss/rss-topstories" },
            { "World News",                  "https://www.cbc.ca/webfeed/rss/rss-world" },
            { "Canada News",                 "https://www.cbc.ca/webfeed/rss/rss-canada" },
            { "Politics News",               "https://www.cbc.ca/webfeed/rss/rss-politics" },
            { "Business News",               "https://www.cbc.ca/webfeed/rss/rss-business" },
            { "Health News",                 "https://www.cbc.ca/webfeed/rss/rss-health" },
            { "Arts News",                   "https://www.cbc.ca/webfeed/rss/rss-arts" },
            { "Technology News",             "https://www.cbc.ca/webfeed/rss/rss-technology" },
            { "Indigenous News",             "https://www.cbc.ca/webfeed/rss/rss-Indigenous" },
            { "Sports News",                 "https://www.cbc.ca/webfeed/rss/rss-sports" },
            { "MLB News",                    "https://www.cbc.ca/webfeed/rss/rss-sports-mlb" },
            { "NBA News",                    "https://www.cbc.ca/webfeed/rss/rss-sports-nba" },
            { "Curling News",                "https://www.cbc.ca/webfeed/rss/rss-sports-curling" },
            { "CFL News",                    "https://www.cbc.ca/webfeed/rss/rss-sports-cfl" },
            { "NFL News",                    "https://www.cbc.ca/webfeed/rss/rss-sports-nfl" },
            { "NHL News",                    "https://www.cbc.ca/webfeed/rss/rss-sports-nhl" },
            { "Soccer News",                 "https://www.cbc.ca/webfeed/rss/rss-sports-soccer" },
            { "Figureskating News",          "https://www.cbc.ca/webfeed/rss/rss-sports-figureskating" },
            { "Golf News",                   "https://www.cbc.ca/webfeed/rss/rss-sports-golf" },
            { "Olympic News",                "https://www.cbc.ca/webfeed/rss/rss-sports-olympics" },
            { "Skiing News",                 "https://www.cbc.ca/webfeed/rss/rss-sports-skiing" },
            { "Tennis News",                 "https://www.cbc.ca/webfeed/rss/rss-sports-tennis" },
            { "British Columbia News",       "https://www.cbc.ca/webfeed/rss/rss-canada-britishcolumbia" },
            { "Kamloops News",               "https://www.cbc.ca/webfeed/rss/rss-canada-kamloops" },
            { "Calgary News",                "https://www.cbc.ca/webfeed/rss/rss-canada-calgary" },
            { "Edmonton News",               "https://www.cbc.ca/webfeed/rss/rss-canada-edmonton" },
            { "Saskatchewan News",           "https://www.cbc.ca/webfeed/rss/rss-canada-saskatchewan" },
            { "Saskatoon News",              "https://www.cbc.ca/webfeed/rss/rss-canada-saskatoon" },
            { "Manitoba News",               "https://www.cbc.ca/webfeed/rss/rss-canada-manitoba" },
            { "Thunderbay News",             "https://www.cbc.ca/webfeed/rss/rss-canada-thunderbay" },
            { "Sudbury News",                "https://www.cbc.ca/webfeed/rss/rss-canada-sudbury" },
            { "Windsor News",                "https://www.cbc.ca/webfeed/rss/rss-canada-windsor" },
            { "London News",                 "https://www.cbc.ca/webfeed/rss/rss-canada-london" },
            { "Kitchener & Waterloo News",   "https://www.cbc.ca/webfeed/rss/rss-canada-kitchenerwaterloo" },
            { "Toronto News",                "https://www.cbc.ca/webfeed/rss/rss-canada-toronto" },
            { "Hamilton News",               "https://www.cbc.ca/webfeed/rss/rss-canada-hamiltonnews" },
            { "Montreal News",               "https://www.cbc.ca/webfeed/rss/rss-canada-montreal" },
            { "New Brunswick News",          "https://www.cbc.ca/webfeed/rss/rss-canada-newbrunswick" },
            { "Prince Edward Island News",   "https://www.cbc.ca/webfeed/rss/rss-canada-pei" },
            { "Nova Scotia News",            "https://www.cbc.ca/webfeed/rss/rss-canada-novascotia" },
            { "Newfoundland & Labrador News","https://www.cbc.ca/webfeed/rss/rss-canada-newfoundland" },
            { "North News",                  "https://www.cbc.ca/webfeed/rss/rss-canada-north" },
            { "Ottawa News",                 "https://www.cbc.ca/webfeed/rss/rss-canada-ottawa" }
        };

        /// <summary>
        /// Exposes the category→URL map.
        /// </summary>
        public IReadOnlyDictionary<string, string> Feeds => _feeds;

        /// <summary>
        /// Fetches and parses the RSS feed from the given URL.
        /// </summary>
        public SyndicationFeed GetFeed(string feedUrl)
        {
            try
            {
                Debug.WriteLine($"[RSSFeedParser] Loading feed from {feedUrl}");
                using (XmlReader reader = XmlReader.Create(feedUrl))
                {
                    var feed = SyndicationFeed.Load(reader);
                    int count = feed?.Items?.Count() ?? 0;
                    Debug.WriteLine($"[RSSFeedParser] Parsed '{feed?.Title?.Text}' with {count} items");
                    return feed;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[RSSFeedParser] Error loading {feedUrl}: {ex.Message}");
                return null;
            }
        }
    }
}
