using System;
using System.Threading.Tasks;

namespace Telefact
{
    /// <summary>
    /// Plays short audio cues for Teletext broadcast events.
    /// Runs beeps on a background thread to avoid blocking the UI.
    /// </summary>
    internal static class AudioManager
    {
        /// <summary>
        /// Short high beep for a page advance within the same category.
        /// </summary>
        public static void PlayPageBeep() =>
            Task.Run(() =>
            {
                try { Console.Beep(880, 60); }
                catch { /* PC speaker unavailable – silent fallback */ }
            });

        /// <summary>
        /// Two-tone jingle when advancing to a new category index page.
        /// </summary>
        public static void PlayCategoryJingle() =>
            Task.Run(() =>
            {
                try
                {
                    Console.Beep(660, 80);
                    System.Threading.Thread.Sleep(50);
                    Console.Beep(1046, 130);
                }
                catch { /* PC speaker unavailable – silent fallback */ }
            });
    }
}
