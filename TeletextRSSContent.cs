using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Telefact
{
    public static class RSSPages
    {
        public static readonly List<string> Categories = new List<string>
        {
            "Top Stories",
            "Canada News",
            "World News",
            // add more categories as needed...
        };

        public const int PagesPerCategory = 10; // 300–309, 310–319, …
        public const int IndexOffset = 0;  // X0 shows the index
        public const int FirstStoryOffset = 1;  // X1–X6 show story pages
        public const int StoriesPerCategory = 6;  // six items per category
    }

    public class TeletextRSSContent
    {
        // Grid constants matching TeletextContent
        private const int PageWidth = 38; // cells
        private const int LeftPadding = 1;  // cells
        private const int RightPadding = 1;  // cells

        private readonly RSSCacheManager _cache;
        private readonly int _pageNumber;

        public TeletextRSSContent(int pageNumber)
        {
            _pageNumber = pageNumber;
            _cache = new RSSCacheManager();
        }

        public void Render(
            Graphics g,
            int clientWidth,
            int clientHeight,
            int headerHeight,
            int footerHeight,
            int cellWidth,
            int cellHeight
        )
        {
            // Figure out category-block index (300→0, 310→1, etc.)
            int blockIndex = (_pageNumber / 10) - 30;
            if (blockIndex < 0 || blockIndex >= RSSPages.Categories.Count)
                return;

            string category = RSSPages.Categories[blockIndex];
            SyndicationFeed feed = _cache.GetFeed(category);
            int sub = _pageNumber % 10;

            if (feed == null)
            {
                Debug.WriteLine($"[TeletextRSSContent] No feed for '{category}'");
                return;
            }

            int totalRows = (clientHeight - headerHeight - footerHeight) / cellHeight;
            int y0 = headerHeight;

            using (var font = new Font("Modeseven", cellHeight * 0.8f))
            {
                // 1) INDEX PAGE (sub == 0)
                if (sub == RSSPages.IndexOffset)
                {
                    int row = 0;
                    int cols = clientWidth / cellWidth;

                    foreach (var entry in feed.Items
                                              .Take(5)
                                              .Select((it, i) => new { it, i }))
                    {
                        if (row >= totalRows) break;

                        // Title in UPPER
                        string rawTitle = entry.it.Title.Text.ToUpperInvariant();
                        // Compute the story page number
                        int blockStart = (_pageNumber / 10) * 10;
                        string pageNum = (blockStart + entry.i + RSSPages.FirstStoryOffset).ToString();

                        // Calculate how many title chars fit before dot-leader + pageNum
                        int maxTitleCols = cols
                                         - LeftPadding
                                         - RightPadding
                                         - pageNum.Length
                                         - 1; // at least one dot

                        // Wrap to up to 2 lines
                        var lines = WordWrap(rawTitle, maxTitleCols)
                                    .Take(2)
                                    .ToList();

                        for (int li = 0; li < lines.Count && row < totalRows; li++)
                        {
                            string text = lines[li];
                            float y = y0 + row * cellHeight;

                            // 1a) draw each char of text in WHITE
                            for (int ci = 0; ci < text.Length; ci++)
                            {
                                float x = (LeftPadding + ci) * cellWidth;
                                g.DrawString(text[ci].ToString(), font, Brushes.White, x, y);
                            }

                            // 1b) if last wrapped line, draw dot-leader + pageNum
                            if (li == lines.Count - 1)
                            {
                                int dotStart = LeftPadding + text.Length;
                                int dotEnd = cols - RightPadding - pageNum.Length - 1;

                                // cyan dots
                                for (int c = dotStart; c <= dotEnd; c++)
                                {
                                    float x = c * cellWidth;
                                    g.DrawString(".", font, Brushes.Cyan, x, y);
                                }

                                // white pageNum, right-aligned
                                for (int c = 0; c < pageNum.Length; c++)
                                {
                                    float x = (cols - RightPadding - pageNum.Length + c) * cellWidth;
                                    g.DrawString(pageNum[c].ToString(), font, Brushes.White, x, y);
                                }
                            }

                            row++;
                        }

                        // blank “breathing” row
                        row++;
                    }
                }
                // 2) STORY PAGE (sub 1..6)
                else
                {
                    int storyIdx = sub - RSSPages.FirstStoryOffset;
                    if (storyIdx < 0 || storyIdx >= RSSPages.StoriesPerCategory)
                        return;

                    var item = feed.Items.ElementAtOrDefault(storyIdx);
                    if (item == null) return;

                    // 2a) Title (UPPERCASE, WHITE)
                    string titleText = item.Title.Text.ToUpperInvariant();
                    var titleLines = WordWrap(titleText, PageWidth).ToList();

                    int drawRow = 0;
                    foreach (var line in titleLines)
                    {
                        if (drawRow >= totalRows) break;
                        float y = y0 + drawRow * cellHeight;

                        for (int ci = 0; ci < line.Length; ci++)
                        {
                            float x = (LeftPadding + ci) * cellWidth;
                            g.DrawString(line[ci].ToString(), font, Brushes.White, x, y);
                        }

                        drawRow++;
                    }

                    // blank line before body
                    if (drawRow < totalRows) drawRow++;

                    // 2b) Body text (original case, CYAN)
                    // get summary or RSS <description>
                    string rawDesc = item.Summary?.Text
                                     ?? item.ElementExtensions
                                            .FirstOrDefault(e => e.OuterName == "description")
                                            ?.GetObject<XElement>()?.Value
                                     ?? "";
                    // strip HTML tags
                    string descText = Regex.Replace(rawDesc, "<.*?>", "");

                    var descLines = WordWrap(descText, PageWidth)
                                    .Take(totalRows - drawRow)
                                    .ToList();

                    foreach (var line in descLines)
                    {
                        if (drawRow >= totalRows) break;
                        float y = y0 + drawRow * cellHeight;

                        for (int ci = 0; ci < line.Length; ci++)
                        {
                            float x = (LeftPadding + ci) * cellWidth;
                            g.DrawString(line[ci].ToString(), font, Brushes.Cyan, x, y);
                        }

                        drawRow++;
                    }

                    // 2c) Subpage counter bottom-right (CYAN)
                    string counter = $"{sub + 1}/{RSSPages.PagesPerCategory}";
                    SizeF sz = g.MeasureString(counter, font);
                    float cx = clientWidth - ((RightPadding * cellWidth) + sz.Width + 5);
                    float cy = clientHeight - footerHeight - sz.Height - 5;
                    g.DrawString(counter, font, Brushes.Cyan, cx, cy);
                }
            }
        }

        /// <summary>
        /// Splits text on word boundaries so no line exceeds maxCols.
        /// </summary>
        private IEnumerable<string> WordWrap(string text, int maxCols)
        {
            var words = text
                        .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var line = new StringBuilder();

            foreach (var w in words)
            {
                int needed = line.Length + (line.Length > 0 ? 1 : 0) + w.Length;
                if (needed > maxCols)
                {
                    yield return line.ToString();
                    line.Clear();
                }
                if (line.Length > 0)
                    line.Append(' ');
                line.Append(w);
            }
            if (line.Length > 0)
                yield return line.ToString();
        }
    }
}
