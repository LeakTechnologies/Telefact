using System;
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

        public void Render(Graphics g, int clientWidth, int clientHeight)
        {
            if (g == null)
                throw new ArgumentNullException(nameof(g));

            // Render header.
            _header.Render(g, clientWidth);

            // Decide fixed grid rows and columns.
            int totalRows = TeletextGrid.TotalRows;
            int totalColumns = TeletextGrid.TotalColumns;

            // Define header and footer rows.
            int headerRows = 2;  // Global header is 2 rows.
            int footerRows = 1;  // Global footer is 1 row.

            // Compute cell sizes – adapt cell size to the window dimensions.
            int cellHeight = clientHeight / totalRows;
            int cellWidth = clientWidth / totalColumns;

            // Calculate header and footer heights in pixels.
            int headerHeight = headerRows * cellHeight;
            int footerHeight = footerRows * cellHeight;

            // Render the content area.
            _content.Render(g, clientWidth, clientHeight, headerHeight, footerHeight, cellWidth, cellHeight);

            // Render the footer.
            _footer.Render(g, clientWidth, clientHeight, cellHeight, cellWidth);
        }

        /// <summary>
        /// Advances to the next subpage in the TeletextContent.
        /// </summary>
        public void NextSubpage()
        {
            _content.NextSubpage();
        }
    }
}
