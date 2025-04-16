using System;
using System.Drawing;

namespace Telefact
{
    public class TeletextFooter
    {
        private const int CellWidth = 20;
        private const int CellHeight = 26;
        private const int PageWidth = 38; // Usable cells for content
        private const int LeftPadding = 1; // Padding cells on the left
        private const int RightPadding = 1; // Padding cells on the right
        private readonly Font Font = new Font("Modeseven", 20f, FontStyle.Regular);

        public void Render(Graphics g, int clientWidth, int clientHeight)
        {
            if (g == null) throw new ArgumentNullException(nameof(g));

            // Footer content
            int rowIndex = (clientHeight - CellHeight) / CellHeight;
            string footerText = $"Footer Line – Row {rowIndex}";

            // Calculate positions
            int footerY = clientHeight - CellHeight;

            // Total width of the grid in pixels
            int totalGridWidth = PageWidth * CellWidth;

            // Set starting X position to align the text to the left side
            int startX = LeftPadding * CellWidth;

            // Draw white background for the 38 usable cells
            using (Brush backgroundBrush = new SolidBrush(Color.White))
            {
                g.FillRectangle(backgroundBrush, LeftPadding * CellWidth, footerY, PageWidth * CellWidth, CellHeight);
            }

            // Draw black padding block on the right side (1 cell)
            using (Brush blackBrush = new SolidBrush(Color.Black))
            {
                int rightPaddingX = (LeftPadding + PageWidth) * CellWidth;
                g.FillRectangle(blackBrush, rightPaddingX, footerY, RightPadding * CellWidth, CellHeight);
            }

            // Draw Footer Text (aligned to the left side of the footer)
            for (int i = 0; i < footerText.Length; i++)
            {
                int charX = startX + (i * CellWidth); // Position each character in its own cell
                g.DrawString(footerText[i].ToString(), Font, Brushes.Red, charX, footerY);
            }
        }
    }
}
