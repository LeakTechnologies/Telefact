using System;
using System.Drawing;

namespace Telefact
{
    public class TeletextContent
    {
        // Cell dimensions (in pixels)
        private const int CellWidth = 20;
        private const int CellHeight = 26;
        // Horizontal setting: Number of cells for content (same as header/footer)
        private const int PageWidth = 38;
        // Vertical padding (number of cells) at top and bottom of the content frame
        private const int VerticalPaddingTop = 1;
        private const int VerticalPaddingBottom = 1;

        private readonly Font _font = new Font("Modeseven", 20f, FontStyle.Regular);

        /// <summary>
        /// Renders the Teletext content between the header and footer.
        /// </summary>
        /// <param name="g">The graphics object.</param>
        /// <param name="clientWidth">Width of the client area.</param>
        /// <param name="clientHeight">Height of the client area.</param>
        /// <param name="headerHeight">Height of the header area in pixels.</param>
        /// <param name="footerHeight">Height of the footer area in pixels.</param>
        public void Render(Graphics g, int clientWidth, int clientHeight, int headerHeight, int footerHeight, int cellWidth, int cellHeight)
        {
            if (g == null)
                throw new ArgumentNullException(nameof(g));

            // Use vertical padding in terms of rows (e.g., 1 row at top and bottom of the content area).
            int verticalPaddingTop = 1;
            int verticalPaddingBottom = 1;

            int contentStartY = headerHeight + verticalPaddingTop * cellHeight;
            int contentEndY = clientHeight - footerHeight - verticalPaddingBottom * cellHeight;
            int contentHeight = contentEndY - contentStartY;

            using (Brush backgroundBrush = new SolidBrush(Color.DarkBlue))
            {
                g.FillRectangle(backgroundBrush, 0, contentStartY, clientWidth, contentHeight);
            }

            // Determine the number of rows allocated for content.
            int contentRows = (TeletextGrid.TotalRows - /* headerRows */ 2 - /* footerRows */ 1) - verticalPaddingTop - verticalPaddingBottom;

            // Draw placeholder text on each line.
            for (int i = 0; i < contentRows; i++)
            {
                int yPos = contentStartY + i * cellHeight;
                string lineText = $"Line {i + 1}: Teletext content description";
                // Draw string with a small horizontal offset relative to cellWidth if desired
                g.DrawString(lineText, new Font("Modeseven", cellHeight * 0.8f), Brushes.Yellow, 10, yPos);
            }
        }
    }
}
