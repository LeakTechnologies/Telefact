using System;
using System.Drawing;

namespace Telefact
{
    public class TeletextHeader
    {
        private const int CellWidth = 20; // Width of a single cell in pixels
        private const int CellHeight = 26; // Height of a single cell in pixels
        private const int PageWidth = 38; // Usable cells for content
        private const int LeftPadding = 1; // Padding cells on the left
        private const int RightPadding = 1; // Padding cells on the right
        private const int TopMargin = 18; // Top margin in pixels
        private readonly Font Font = new Font("Modeseven", 20f, FontStyle.Regular);

        /// <summary>
        /// Renders the Teletext header.
        /// IMPORTANT: Do not modify this method without explicit permission.
        /// The header layout has been finalized and must remain consistent.
        /// </summary>
        public void Render(Graphics g, int clientWidth)
        {
            if (g == null) throw new ArgumentNullException(nameof(g));

            // Header content
            string pageNumberLeft = " P100"; // Indented by one cell
            string serviceName = "Telefact";
            string pageNumberRight = "100";
            string timestamp = DateTime.Now.ToString("MMM dd HH:mm:ss");

            // Font padding constants
            int serviceNamePadding = 2;
            int headerY = TopMargin;

            // X start position
            int startX = LeftPadding * CellWidth;

            // Timestamp right-aligned
            int timestampX = (LeftPadding + PageWidth - timestamp.Length) * CellWidth;

            // Page number right: 1 cell left of timestamp
            int pageNumberRightX = timestampX - ((pageNumberRight.Length + 1) * CellWidth);

            // Service name starts after page number left + 1 cell gap
            int serviceNameX = startX + (pageNumberLeft.Length + 1) * CellWidth;

            // Service name background width
            int serviceNameBackgroundWidth = (serviceName.Length + (2 * serviceNamePadding)) * CellWidth;

            // Draw Page Number Left
            for (int i = 0; i < pageNumberLeft.Length; i++)
            {
                int charX = startX + (i * CellWidth);
                g.DrawString(pageNumberLeft[i].ToString(), Font, Brushes.Green, charX, headerY);
            }

            // Draw Service Name Background
            using (Brush backgroundBrush = new SolidBrush(Color.Red))
            {
                g.FillRectangle(backgroundBrush, serviceNameX, headerY, serviceNameBackgroundWidth, CellHeight);
            }

            // Draw Service Name Text
            for (int i = 0; i < serviceName.Length; i++)
            {
                int charX = serviceNameX + ((i + serviceNamePadding) * CellWidth);
                g.DrawString(serviceName[i].ToString(), Font, Brushes.Yellow, charX, headerY);
            }

            // Draw Page Number Right (1 cell left of timestamp)
            for (int i = 0; i < pageNumberRight.Length; i++)
            {
                int charX = pageNumberRightX + (i * CellWidth);
                g.DrawString(pageNumberRight[i].ToString(), Font, Brushes.White, charX, headerY);
            }

            // Draw Timestamp (right aligned)
            for (int i = 0; i < timestamp.Length; i++)
            {
                int charX = timestampX + (i * CellWidth);
                g.DrawString(timestamp[i].ToString(), Font, Brushes.Yellow, charX, headerY);
            }
        }
    }
}
