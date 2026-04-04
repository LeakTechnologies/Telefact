using System;
using System.Diagnostics;
using System.Drawing;

namespace Telefact
{
    public static class TeletextGrid
    {
        public const int TotalColumns = 40;
        public const int TotalRows = 25;
    }

    public class Renderer
    {
        private readonly TeletextHeader _header = new TeletextHeader();
        private readonly TeletextFooter _footer = new TeletextFooter();
        private readonly TeletextContent _content = new TeletextContent();

        /// <summary>
        /// Current Teletext page number (e.g., 100, 300, 301, …).
        /// </summary>
        public int PageNumber { get; set; } = 100;

        /// <summary>
        /// Broadcast loop progress for the current page (0.0 = just loaded, 1.0 = about to advance).
        /// Used to render the on-screen progress bar.
        /// </summary>
        public double PageProgress { get; set; }

        public void Render(Graphics g, int clientWidth, int clientHeight)
        {
            Debug.WriteLine($"[Renderer] Render(page={PageNumber}, size={clientWidth}×{clientHeight})");

            // 1) Global header
            _header.Render(g, clientWidth, PageNumber);

            // 2) Grid metrics
            int totalRows = TeletextGrid.TotalRows;
            int totalCols = TeletextGrid.TotalColumns;
            int cellHeight = clientHeight / totalRows;
            int cellWidth = clientWidth / totalCols;
            int headerHeight = 2 * cellHeight;
            int footerHeight = 1 * cellHeight;

            // 3) Decide branch
            const int baseRSSPage = 300;
            int offset = PageNumber - baseRSSPage;
            int blockIndex = offset / RSSPages.PagesPerCategory;
            bool isRSSPage = PageNumber >= baseRSSPage
                             && blockIndex >= 0
                             && blockIndex < RSSPages.Categories.Count;

            if (isRSSPage)
            {
                Debug.WriteLine($"[Renderer] RSS branch: page={PageNumber}");
                var rssRenderer = new TeletextRSSContent(PageNumber);
                rssRenderer.Render(
                    g,
                    clientWidth, clientHeight,
                    headerHeight, footerHeight,
                    cellWidth, cellHeight
                );
            }
            else if (PageNumber == 777 && ConfigManager.DebugStaticStoryEnabled)
            {
                Debug.WriteLine($"[Renderer] StaticStory branch: page={PageNumber}");
                _content.Render(
                    g,
                    clientWidth, clientHeight,
                    headerHeight, footerHeight,
                    cellWidth, cellHeight
                );
            }
            else
            {
                Debug.WriteLine($"[Renderer] NoContent branch: page={PageNumber}");
                g.FillRectangle(
                    Brushes.Black,
                    0, headerHeight,
                    clientWidth,
                    clientHeight - headerHeight - footerHeight
                );
            }

            // 4) Broadcast progress bar (4 px strip just above footer)
            RenderProgressBar(g, clientWidth, clientHeight, footerHeight);

            // 5) Global footer
            _footer.Render(g, clientWidth, clientHeight, cellHeight, cellWidth);
        }

        public void NextSubpage()
        {
            if (PageNumber == 100 || PageNumber == 777)
                _content.NextSubpage();
        }

        private void RenderProgressBar(Graphics g, int clientWidth, int clientHeight, int footerHeight)
        {
            const int barHeight = 4;
            int barY = clientHeight - footerHeight - barHeight;
            double clampedProgress = Math.Max(0.0, Math.Min(1.0, PageProgress));
            int filledWidth = (int)(clientWidth * clampedProgress);

            // Dark background for the full bar slot
            g.FillRectangle(Brushes.Black, 0, barY, clientWidth, barHeight);

            // Cyan fill showing elapsed time within the current page
            if (filledWidth > 0)
                g.FillRectangle(Brushes.Cyan, 0, barY, filledWidth, barHeight);
        }
    }
}
