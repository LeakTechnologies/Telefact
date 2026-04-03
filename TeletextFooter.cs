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
        private const int PageWidth = 38;
        private const int LeftPadding = 1;
        private const int RightPadding = 1;

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

            int footerRowIndex = TeletextGrid.TotalRows - 1;
            int footerY = footerRowIndex * cellHeight;
            int startX = LeftPadding * cellWidth;

            // Navigation hints for the active RSS category blocks.
            string footerText = "300 NEWS  310 CANADA  320 WORLD";

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

            using (var font = new Font("Modeseven", cellHeight * 0.8f, FontStyle.Regular))
            {
                for (int i = 0; i < footerText.Length; i++)
                {
                    int charX = startX + (i * cellWidth);
                    g.DrawString(footerText[i].ToString(), font, Brushes.Black, charX, footerY);
                }
            }
        }
    }
}
