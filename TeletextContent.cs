using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Telefact
{
    public class TeletextContent
    {
        // This internal "page header" (not the global TeletextHeader)
        // takes up 3 rows in the content area.
        private const int PageHeaderRows = 3;

        // Match TeletextHeader/TeletextFooter constants:
        private const int PageWidth = 38;     // Usable content cells horizontally
        private const int LeftPadding = 1;    // 1 cell of padding on the left
        private const int RightPadding = 1;   // 1 cell of padding on the right

        // The full text of the Teletext history story.
        private readonly string _story =
@"Teletext History: The Dawn of Teletext

In the early 1970s, when broadcast television was the primary source of home entertainment and information,
an ingenious idea was born to deliver data alongside the picture. This innovation came to be known as Teletext.
Pioneered in the United Kingdom by the BBC, Teletext began as a way to broadcast simple text and graphics
over unused portions of the TV signal. Viewers could access news, weather, sports scores, and even stock
prices—all via their television sets.

Teletext History: The Technological Evolution

As Teletext services gained popularity, their technology evolved rapidly. By the late 1970s and early 1980s,
systems such as Ceefax in the UK and ORACLE in other parts of Europe brought a new era of interactive information
services directly into homes. The system relied on a grid-based layout, reminiscent of a digital mosaic. Each
screen was composed of a fixed number of cells, enabling efficient use of limited bandwidth and modest graphics.

Teletext History: Legacy and Modern Impact

Though the rise of the internet has replaced many functions that Teletext once served, its design and technology
left a lasting imprint on digital information systems. Today, Teletext is remembered for its technical ingenuity
and its role in democratizing access to information long before smartphones and broadband became ubiquitous.
Its grid-based aesthetic and color-coded presentation continue to influence modern designs focused on clarity,
simplicity, and accessibility.";

        // List of subpages (each is an array of lines).
        private List<string[]> _subpages = new List<string[]>();
        // Index of the currently displayed subpage.
        private int _currentSubpage = 0;

        // This internal page header content shows at the top of every subpage inside this content area.
        private readonly string[] _pageHeaderContent = new string[]
        {
            "---------",
            "Top Stories",
            "---------"
        };

        /// <summary>
        /// Prepares subpages by word-wrapping the story text, then chunking it into pages.
        /// The number of content lines per page is (availableRows - PageHeaderRows).
        /// 
        /// We also clamp the maximum line length to 'PageWidth' for consistent padding.
        /// </summary>
        /// <param name="availableRows">Total number of text rows in the TeletextContent area.</param>
        public void PrepareSubpages(int availableRows)
        {
            // The number of rows that remain after the internal page header.
            int storyRows = availableRows - PageHeaderRows;
            if (storyRows < 1)
                return;

            // Word-wrap lines to fit PageWidth columns
            List<string> wrappedLines = WordWrapStory(_story, PageWidth);

            // Split wrapped lines into subpages of 'storyRows'
            _subpages.Clear();
            for (int i = 0; i < wrappedLines.Count; i += storyRows)
            {
                string[] pageLines = wrappedLines.Skip(i).Take(storyRows).ToArray();
                _subpages.Add(pageLines);
            }
        }

        /// <summary>
        /// Renders the TeletextContent area with a black background (only in the region below the global header).
        /// Draws a 3-line internal page header at the top, then the story lines, and a subpage counter bottom-right.
        /// </summary>
        /// <param name="g">Graphics to draw with.</param>
        /// <param name="clientWidth">Width of the form's client area.</param>
        /// <param name="clientHeight">Height of the form's client area.</param>
        /// <param name="headerHeight">The global TeletextHeader height in pixels.</param>
        /// <param name="footerHeight">The global TeletextFooter height in pixels.</param>
        /// <param name="cellWidth">Computed width of each cell in pixels (from Renderer).</param>
        /// <param name="cellHeight">Computed height of each cell in pixels (from Renderer).</param>
        public void Render(Graphics g, int clientWidth, int clientHeight,
                           int headerHeight, int footerHeight,
                           int cellWidth, int cellHeight)
        {
            if (g == null) throw new ArgumentNullException(nameof(g));

            // Determine the vertical space for TeletextContent (excluding global header and footer).
            int contentAreaY = headerHeight;
            int contentAreaHeight = clientHeight - headerHeight - footerHeight;

            // Fill only the content region with black, so we don't overwrite the global TeletextHeader.
            g.FillRectangle(Brushes.Black, 0, contentAreaY, clientWidth, contentAreaHeight);

            // Compute total text rows in this content area.
            int totalRows = contentAreaHeight / cellHeight;
            if (totalRows < PageHeaderRows) return;

            // Prepare subpages if not already done (or if row count changed).
            if (_subpages.Count == 0)
            {
                PrepareSubpages(totalRows);
            }

            // Draw the 3-line page header in cyan, respecting left padding.
            // We assume that the user wants the same left padding as the TeletextHeader & TeletextFooter.
            using (Font headerFont = new Font("Modeseven", cellHeight * 0.8f))
            {
                for (int i = 0; i < PageHeaderRows; i++)
                {
                    int yPos = contentAreaY + i * cellHeight;
                    // Start text at (LeftPadding cells).
                    float xPos = LeftPadding * cellWidth;
                    g.DrawString(_pageHeaderContent[i], headerFont, Brushes.Cyan, new PointF(xPos, yPos));
                }
            }

            // Where story content begins (below the internal page header).
            int storyStartY = contentAreaY + (PageHeaderRows * cellHeight);

            // Fetch the current subpage lines (if any).
            if (_subpages.Count == 0)
                return;

            string[] storyLines = _subpages[_currentSubpage];

            // Draw the story lines in cyan, each line respecting the left padding.
            using (Font contentFont = new Font("Modeseven", cellHeight * 0.8f))
            {
                for (int i = 0; i < storyLines.Length; i++)
                {
                    int yPos = storyStartY + i * cellHeight;
                    float xPos = LeftPadding * cellWidth;
                    g.DrawString(storyLines[i], contentFont, Brushes.Cyan, new PointF(xPos, yPos));
                }

                // Draw the subpage counter in the bottom right corner of the TeletextContent region.
                string counterText = $"{_currentSubpage + 1}/{_subpages.Count}";
                SizeF counterSize = g.MeasureString(counterText, contentFont);

                // Align with the content area, subtract the RightPadding cells.
                float counterX = clientWidth - (RightPadding * cellWidth) - counterSize.Width - 5;
                float counterY = (contentAreaY + contentAreaHeight) - counterSize.Height - 5;
                g.DrawString(counterText, contentFont, Brushes.Cyan, new PointF(counterX, counterY));
            }
        }

        /// <summary>
        /// Advances to the next subpage in a round-robin fashion.
        /// </summary>
        public void NextSubpage()
        {
            if (_subpages.Count == 0) return;
            _currentSubpage = (_currentSubpage + 1) % _subpages.Count;
        }

        /// <summary>
        /// Helper method: word-wraps a block of text to the given maximum line width (in characters).
        /// </summary>
        /// <param name="text">The full story text.</param>
        /// <param name="maxColumns">Maximum number of characters per line.</param>
        /// <returns>A list of wrapped lines.</returns>
        private List<string> WordWrapStory(string text, int maxColumns)
        {
            var lines = new List<string>();

            // Split the text by newline to handle paragraphs.
            string[] rawLines = text.Split(new[] { '\n' }, StringSplitOptions.None);

            foreach (string rawLine in rawLines)
            {
                string trimmedLine = rawLine.TrimEnd();
                // If it's empty, keep a blank line.
                if (string.IsNullOrEmpty(trimmedLine))
                {
                    lines.Add("");
                    continue;
                }

                // Word-wrap this line.
                string[] words = trimmedLine.Split(' ');
                var currentLine = new StringBuilder();

                foreach (string word in words)
                {
                    // If adding the next word exceeds maxColumns, finalize currentLine.
                    if (currentLine.Length + (currentLine.Length > 0 ? 1 : 0) + word.Length > maxColumns)
                    {
                        lines.Add(currentLine.ToString());
                        currentLine.Clear();
                    }

                    if (currentLine.Length > 0)
                        currentLine.Append(" ");
                    currentLine.Append(word);
                }

                // Flush any remainder.
                if (currentLine.Length > 0)
                {
                    lines.Add(currentLine.ToString());
                }
            }

            return lines;
        }
    }
}
