using System;
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

        // List of subpages, each subpage is an array of lines (already word-wrapped).
        private List<string[]> _subpages = new List<string[]>();
        // Current subpage index.
        private int _currentSubpage = 0;

        // The 3-line page header content (drawn at the top of the content area).
        private readonly string[] _pageHeaderContent =
        {
            "---------",
            "Top Stories",
            "---------"
        };

        /// <summary>
        /// Prepares subpages by wrapping lines within PageWidth columns and splitting them
        /// into multiple pages if necessary. The actual story rows per page is:
        ///   storyRows = (availableRows - PageHeaderRows - BottomBlankRow).
        /// </summary>
        public void PrepareSubpages(int availableRows)
        {
            int storyRows = availableRows - PageHeaderRows - BottomBlankRow;
            if (storyRows < 1) return;

            List<string> wrappedLines = WordWrapStory(_story, PageWidth);

            _subpages.Clear();
            for (int i = 0; i < wrappedLines.Count; i += storyRows)
            {
                string[] pageLines = wrappedLines.Skip(i).Take(storyRows).ToArray();
                _subpages.Add(pageLines);
            }
        }

        /// <summary>
        /// Renders the TeletextContent area:
        /// - Draws a 3-line page header (centered, character-by-character, in yellow).
        /// - Draws the story content (character-by-character, in cyan) with left padding.
        /// - Leaves the bottom row empty for breathing room.
        /// - Displays a subpage counter in the bottom-right corner.
        /// </summary>
        public void Render(Graphics g, int clientWidth, int clientHeight,
                           int headerHeight, int footerHeight,
                           int cellWidth, int cellHeight)
        {
            if (g == null)
                throw new ArgumentNullException(nameof(g));

            // Determine how tall our content area is.
            int contentAreaY = headerHeight;
            int contentAreaHeight = clientHeight - headerHeight - footerHeight;

            // Fill content area with black (avoiding the global header).
            g.FillRectangle(Brushes.Black, 0, contentAreaY, clientWidth, contentAreaHeight);

            // Compute how many rows we can fit in this content area.
            int totalAvailableRows = contentAreaHeight / cellHeight;
            if (totalAvailableRows < (PageHeaderRows + BottomBlankRow))
                return;

            // Prepare subpages if needed.
            if (_subpages.Count == 0)
                PrepareSubpages(totalAvailableRows);

            // 1) Draw the 3-line header character-by-character.
            using (Font headerFont = new Font("Modeseven", cellHeight * 0.8f))
            {
                // How many columns fit horizontally.
                int availableColumns = clientWidth / cellWidth;

                for (int row = 0; row < PageHeaderRows; row++)
                {
                    string headerLine = _pageHeaderContent[row];
                    // Center the header line.
                    int startCol = (availableColumns - headerLine.Length) / 2;
                    int yPos = contentAreaY + row * cellHeight;

                    // Draw each character in its own grid cell.
                    for (int col = 0; col < headerLine.Length; col++)
                    {
                        float xPos = (startCol + col) * cellWidth;
                        g.DrawString(headerLine[col].ToString(), headerFont, Brushes.Yellow, xPos, yPos);
                    }
                }
            }

            // 2) Draw the story content area.
            int storyStartY = contentAreaY + (PageHeaderRows * cellHeight);
            int effectiveStoryRows = totalAvailableRows - PageHeaderRows - BottomBlankRow;

            if (_subpages.Count == 0)
                return;

            // Current subpage lines.
            string[] storyLines = _subpages[_currentSubpage];

            // Draw them character-by-character within the grid.
            using (Font contentFont = new Font("Modeseven", cellHeight * 0.8f))
            {
                // How many columns fit horizontally.
                int availableColumns = clientWidth / cellWidth;

                for (int row = 0; row < storyLines.Length && row < effectiveStoryRows; row++)
                {
                    string line = storyLines[row];
                    int yPos = storyStartY + row * cellHeight;

                    // We start at LeftPadding for each line.
                    int startCol = LeftPadding;
                    for (int col = 0; col < line.Length; col++)
                    {
                        // Make sure we don't exceed the total available columns.
                        if ((startCol + col) >= availableColumns)
                            break;

                        float xPos = (startCol + col) * cellWidth;
                        g.DrawString(line[col].ToString(), contentFont, Brushes.Cyan, xPos, yPos);
                    }
                }

                // 3) Subpage counter in the bottom-right corner.
                string counterText = $"{_currentSubpage + 1}/{_subpages.Count}";
                SizeF counterSize = g.MeasureString(counterText, contentFont);
                float counterX = clientWidth - (RightPadding * cellWidth) - counterSize.Width - 5;
                float counterY = (contentAreaY + contentAreaHeight) - counterSize.Height - 5;
                g.DrawString(counterText, contentFont, Brushes.Cyan, counterX, counterY);
            }
        }

        /// <summary>
        /// Advances to the next subpage of text (round-robin).
        /// </summary>
        public void NextSubpage()
        {
            if (_subpages.Count == 0) return;
            _currentSubpage = (_currentSubpage + 1) % _subpages.Count;
        }

        /// <summary>
        /// Splits the _story text by paragraphs and then word-wraps each paragraph to 'maxColumns'.
        /// Returns a list of lines (not yet separated into pages).
        /// </summary>
        private List<string> WordWrapStory(string text, int maxColumns)
        {
            var lines = new List<string>();
            string[] rawLines = text.Split(new[] { '\n' }, StringSplitOptions.None);

            foreach (string rawLine in rawLines)
            {
                // Trim trailing spaces but keep blank lines intact.
                string trimmedLine = rawLine.TrimEnd();
                if (string.IsNullOrEmpty(trimmedLine))
                {
                    lines.Add("");
                    continue;
                }

                string[] words = trimmedLine.Split(' ');
                var currentLine = new StringBuilder();

                foreach (string word in words)
                {
                    // +1 for the space if we append one.
                    int nextLength = currentLine.Length + (currentLine.Length > 0 ? 1 : 0) + word.Length;
                    if (nextLength > maxColumns)
                    {
                        lines.Add(currentLine.ToString());
                        currentLine.Clear();
                    }
                    if (currentLine.Length > 0)
                        currentLine.Append(' ');
                    currentLine.Append(word);
                }

                if (currentLine.Length > 0)
                    lines.Add(currentLine.ToString());
            }

            return lines;
        }
    }
}
