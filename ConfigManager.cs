using System.Configuration;
using System.Diagnostics;

namespace Telefact
{
    public static class ConfigManager
    {
        // Read once at startup; config values do not change at runtime.
        public static readonly bool DebugStaticStoryEnabled = ReadDebugStaticStory();

        private static bool ReadDebugStaticStory()
        {
            string raw = ConfigurationManager.AppSettings["DebugStaticStoryEnabled"];
            bool flag = bool.TryParse(raw, out var v) && v;
            Debug.WriteLine($"[ConfigManager] DebugStaticStoryEnabled = {flag}");
            return flag;
        }
    }
}
