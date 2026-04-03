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

        // Cached RSS renderer — recreated only when the page number changes.
        private TeletextRSSContent _rssContent;
        private int _lastRssPage = -1;

        /// <summary>
        /// Current Teletext page number (e.g., 100, 300, 301, …).
        /// </summary>
        public int PageNumber { get; set; } = 100;

        public void Render(Graphics g, int clientWidth, int clientHeight)
        {
            Debug.WriteLine($"[Renderer] Render(page={PageNumber}, size={clientWidth}x{clientHeight})");

            // 1) Grid metrics
            int totalRows = TeletextGrid.TotalRows;
            int totalCols = TeletextGrid.TotalColumns;
            int cellHeight = clientHeight / totalRows;
            int cellWidth = clientWidth / totalCols;
            int headerHeight = 2 * cellHeight;
            int footerHeight = 1 * cellHeight;

            // 2) Global header (now receives dynamic cell sizing)
            _header.Render(g, clientWidth, PageNumber, cellWidth, cellHeight);

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

                if (_lastRssPage != PageNumber)
                {
                    _rssContent = new TeletextRSSContent(PageNumber);
                    _lastRssPage = PageNumber;
                }

                _rssContent.Render(
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

            // 4) Global footer
            _footer.Render(g, clientWidth, clientHeight, cellHeight, cellWidth);
        }

        public void NextSubpage()
        {
            if (PageNumber == 777)
                _content.NextSubpage();
        }
    }
}
