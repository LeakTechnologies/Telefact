using System;
using System.Drawing;

namespace Telefact
{
    public class TeletextHeader
    {
        private const int CellWidth = 20; // Width of a single cell in pixels
        private const int CellHeight = 26; // Height of a single cell in pixels
        private const int PageWidth = 38; // Usable cells for content
        private const int LeftPadding = 1;  // Padding cells on the left
        private const int RightPadding = 1;  // Padding cells on the right
        private const int TopMargin = 18; // Top margin in pixels
        private readonly Font Font = new Font("Modeseven", 20f, FontStyle.Regular);

        /// <summary>
        /// Renders the Teletext header.
        /// IMPORTANT: Do not modify this method without explicit permission.
        /// The header layout has been finalized and must remain consistent.
        /// </summary>
        public void Render(Graphics g, int clientWidth, int pageNumber)
        {
            if (g == null) throw new ArgumentNullException(nameof(g));

            // Header content: now using the dynamic pageNumber
            string pageNumberLeft = " P" + pageNumber;        // e.g. " P100"
            string serviceName = "Telefact";
            string pageNumberRight = pageNumber.ToString();    // e.g. "100"
            string timestamp = DateTime.Now.ToString("MMM dd HH:mm:ss");

            // Font padding constants
            int serviceNamePadding = 2;
            int headerY = TopMargin;
            int startX = LeftPadding * CellWidth;

            // Timestamp right-aligned
            int timestampX = (LeftPadding + PageWidth - timestamp.Length) * CellWidth;

            // Page number right: sits one cell left of the timestamp
            int pageNumberRightX = timestampX - ((pageNumberRight.Length + 1) * CellWidth);

            // Service name rectangle starts after the left page number + one cell gap
            int serviceNameX = startX + (pageNumberLeft.Length + 1) * CellWidth;
            int serviceNameBackgroundWidth = (serviceName.Length + (2 * serviceNamePadding)) * CellWidth;

            // 1) Draw Page Number Left (green)
            for (int i = 0; i < pageNumberLeft.Length; i++)
            {
                int charX = startX + (i * CellWidth);
                g.DrawString(pageNumberLeft[i].ToString(), Font, Brushes.Green, charX, headerY);
            }

            // 2) Draw Service Name Background (red)
            using (Brush backgroundBrush = new SolidBrush(Color.Red))
            {
                g.FillRectangle(backgroundBrush, serviceNameX, headerY, serviceNameBackgroundWidth, CellHeight);
            }

            // 3) Draw Service Name Text (yellow)
            for (int i = 0; i < serviceName.Length; i++)
            {
                int charX = serviceNameX + ((i + serviceNamePadding) * CellWidth);
                g.DrawString(serviceName[i].ToString(), Font, Brushes.Yellow, charX, headerY);
            }

            // 4) Draw Page Number Right (white)
            for (int i = 0; i < pageNumberRight.Length; i++)
            {
                int charX = pageNumberRightX + (i * CellWidth);
                g.DrawString(pageNumberRight[i].ToString(), Font, Brushes.White, charX, headerY);
            }

            // 5) Draw Timestamp (yellow, right-aligned)
            for (int i = 0; i < timestamp.Length; i++)
            {
                int charX = timestampX + (i * CellWidth);
                g.DrawString(timestamp[i].ToString(), Font, Brushes.Yellow, charX, headerY);
            }
        }
    }
}
