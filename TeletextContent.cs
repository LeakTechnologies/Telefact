using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Telefact
{
    public class TeletextContent
    {
        // Internal page header takes up 3 rows.
        private const int PageHeaderRows = 3;
        // Reserve 1 extra row at the bottom for breathing room.
        private const int BottomBlankRow = 1;

        // Match header/footer left/right padding.
        private const int PageWidth = 38; // Maximum usable columns for content alignment.
        private const int LeftPadding = 1;
        private const int RightPadding = 1;

        // Default “Top Stories” header.
        private readonly string[] _defaultHeader =
        {
            "---------",
            "Top Stories",
            "---------"
        };

        // Debug “Story of Teletext” header.
        private readonly string[] _debugHeader =
        {
            "---------",
            "Story of Teletext",
            "---------"
        };

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
services directly into homes. The system relied on a grid-based layout, enabling efficient use of limited 
bandwidth and modest graphics.

Teletext History: Legacy and Modern Impact

Though the rise of the internet has replaced many functions that Teletext once served, its design and technology 
left a lasting imprint on digital information systems. Today, Teletext is remembered for its technical ingenuity 
and its role in democratizing access to information long before smartphones and broadband became ubiquitous. 
Its grid-based aesthetic and color-coded presentation continue to influence modern designs focused on clarity,
simplicity, and accessibility.";

        // Wrapped subpages
        private List<string[]> _subpages = new List<string[]>();
        private int _currentSubpage = 0;

        public void PrepareSubpages(int availableRows)
        {
            int storyRows = availableRows - PageHeaderRows - BottomBlankRow;
            if (storyRows < 1) return;

            var wrapped = WordWrapStory(_story, PageWidth);
            _subpages.Clear();
            for (int i = 0; i < wrapped.Count; i += storyRows)
                _subpages.Add(wrapped.Skip(i).Take(storyRows).ToArray());
        }

        public void Render(Graphics g,
                           int clientWidth, int clientHeight,
                           int headerHeight, int footerHeight,
                           int cellWidth, int cellHeight)
        {
            Debug.WriteLine($"[TeletextContent] Render subpage={_currentSubpage + 1}/{_subpages.Count}");

            if (g == null) throw new ArgumentNullException(nameof(g));

            int contentY = headerHeight;
            int contentH = clientHeight - headerHeight - footerHeight;
            g.FillRectangle(Brushes.Black, 0, contentY, clientWidth, contentH);

            int totalRows = contentH / cellHeight;
            if (totalRows < PageHeaderRows + BottomBlankRow) return;
            if (_subpages.Count == 0) PrepareSubpages(totalRows);

            // Choose header based on debug flag
            string[] headerLines = ConfigManager.DebugStaticStoryEnabled
                ? _debugHeader      // {"---------","Story of Teletext","---------" }
                : _defaultHeader;   // {"---------","Top Stories","---------"}

            // Draw header character-by-character, centered
            using (var headerFont = new Font("Modeseven", cellHeight * 0.8f))
            {
                int cols = clientWidth / cellWidth;
                for (int r = 0; r < PageHeaderRows; r++)
                {
                    string line = headerLines[r];
                    int startCol = (cols - line.Length) / 2;
                    int yPos = contentY + r * cellHeight;
                    for (int c = 0; c < line.Length; c++)
                    {
                        float xPos = (startCol + c) * cellWidth;
                        g.DrawString(line[c].ToString(), headerFont, Brushes.Yellow, xPos, yPos);
                    }
                }
            }

            // Draw story content
            int storyY = contentY + PageHeaderRows * cellHeight;
            int storyRows = totalRows - PageHeaderRows - BottomBlankRow;
            string[] page = _subpages[_currentSubpage];

            using (var contentFont = new Font("Modeseven", cellHeight * 0.8f))
            {
                int cols = clientWidth / cellWidth;
                for (int r = 0; r < page.Length && r < storyRows; r++)
                {
                    string line = page[r];
                    int yPos = storyY + r * cellHeight;
                    int startCol = LeftPadding;
                    for (int c = 0; c < line.Length && (startCol + c) < cols; c++)
                    {
                        float xPos = (startCol + c) * cellWidth;
                        g.DrawString(line[c].ToString(), contentFont, Brushes.Cyan, xPos, yPos);
                    }
                }

                // Subpage counter
                string counter = $"{_currentSubpage + 1}/{_subpages.Count}";
                SizeF cs = g.MeasureString(counter, contentFont);
                float cx = clientWidth - (RightPadding * cellWidth) - cs.Width - 5;
                float cy = contentY + contentH - cs.Height - 5;
                g.DrawString(counter, contentFont, Brushes.Cyan, cx, cy);
            }
        }

        public void NextSubpage()
        {
            if (_subpages.Count == 0) return;
            _currentSubpage = (_currentSubpage + 1) % _subpages.Count;
        }

        private List<string> WordWrapStory(string text, int maxCols)
        {
            var lines = new List<string>();
            foreach (var raw in text.Split('\n'))
            {
                var t = raw.TrimEnd();
                if (string.IsNullOrEmpty(t)) { lines.Add(""); continue; }
                var sb = new StringBuilder();
                foreach (var w in t.Split(' '))
                {
                    if (sb.Length + (sb.Length > 0 ? 1 : 0) + w.Length > maxCols)
                    {
                        lines.Add(sb.ToString());
                        sb.Clear();
                    }
                    if (sb.Length > 0) sb.Append(' ');
                    sb.Append(w);
                }
                if (sb.Length > 0) lines.Add(sb.ToString());
            }
            return lines;
        }
    }
}
