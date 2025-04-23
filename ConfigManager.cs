using System;
using System.Configuration;
using System.Diagnostics;

namespace Telefact
{
    public static class ConfigManager
    {
        public static bool DebugStaticStoryEnabled
        {
            get
            {
                string raw = ConfigurationManager.AppSettings["DebugStaticStoryEnabled"];
                bool flag = bool.TryParse(raw, out var v) && v;
                Debug.WriteLine($"[ConfigManager] DebugStaticStoryEnabled = {flag}");
                return flag;
            }
        }
    }
}
