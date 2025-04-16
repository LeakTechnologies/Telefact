using System;
using System.Drawing;

namespace Telefact
{
    /// <summary>
    /// TeletextFooter:
    /// This class implements the footer rendering logic.
    /// This code is a protected resource and should not be modified without explicit permission.
    /// </summary>
    public sealed class TeletextFooter
    {
        private const int PageWidth = 38;  // Usable cells for content.
        private const int LeftPadding = 1; // Padding cells on the left.
        private const int RightPadding = 1; // Padding cells on the right.

        private readonly Font Font = new Font("Modeseven", 20f, FontStyle.Regular);

        /// <summary>
        /// Renders the Teletext footer with a fixed grid layout.
        /// </summary>
        /// <param name="g">Graphics object for drawing.</param>
        /// <param name="clientWidth">The width of the client window.</param>
        /// <param name="clientHeight">The height of the client window.</param>
        /// <param name="cellHeight">The computed height of a grid cell.</param>
        /// <param name="cellWidth">The computed width of a grid cell.</param>
        public void Render(Graphics g, int clientWidth, int clientHeight, int cellHeight, int cellWidth)
        {
            if (g == null)
                throw new ArgumentNullException(nameof(g));

            // Set a fixed total number of rows (for example, 25 rows is a common Teletext standard).
            int totalRows = 25;
            // Footer is assumed to be the very last row in the grid.
            int footerRowIndex = totalRows - 1;
            int footerY = footerRowIndex * cellHeight;

            // Prepare the footer text using the fixed grid row index.
            string footerText = $"Footer Line – Row {footerRowIndex}";
            int startX = LeftPadding * cellWidth;

            // Draw a white background for the usable cells in the footer.
            using (Brush backgroundBrush = new SolidBrush(Color.White))
            {
                g.FillRectangle(backgroundBrush, LeftPadding * cellWidth, footerY, PageWidth * cellWidth, cellHeight);
            }

            // Draw a black padding block on the right (1 cell wide).
            using (Brush blackBrush = new SolidBrush(Color.Black))
            {
                int rightPaddingX = (LeftPadding + PageWidth) * cellWidth;
                g.FillRectangle(blackBrush, rightPaddingX, footerY, RightPadding * cellWidth, cellHeight);
            }

            // Render the footer text, printing each character in its own grid cell.
            for (int i = 0; i < footerText.Length; i++)
            {
                int charX = startX + (i * cellWidth);
                g.DrawString(footerText[i].ToString(), Font, Brushes.Red, charX, footerY);
            }
        }
    }
}
