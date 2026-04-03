using System;
using System.Drawing;

namespace Telefact
{
    public class TeletextHeader
    {
        private const int PageWidth = 38;
        private const int LeftPadding = 1;
        private const int RightPadding = 1;

        /// <summary>
        /// Renders the Teletext header.
        /// IMPORTANT: Do not modify this method without explicit permission.
        /// The header layout has been finalized and must remain consistent.
        /// </summary>
        public void Render(Graphics g, int clientWidth, int pageNumber, int cellWidth, int cellHeight)
        {
            if (g == null) throw new ArgumentNullException(nameof(g));

            string pageNumberLeft = " P" + pageNumber;
            string serviceName = "Telefact";
            string pageNumberRight = pageNumber.ToString();
            string timestamp = DateTime.Now.ToString("MMM dd HH:mm:ss");

            int serviceNamePadding = 2;
            int headerY = 0;
            int startX = LeftPadding * cellWidth;

            int timestampX = (LeftPadding + PageWidth - timestamp.Length) * cellWidth;
            int pageNumberRightX = timestampX - ((pageNumberRight.Length + 1) * cellWidth);
            int serviceNameX = startX + (pageNumberLeft.Length + 1) * cellWidth;
            int serviceNameBackgroundWidth = (serviceName.Length + (2 * serviceNamePadding)) * cellWidth;

            using (var font = new Font("Modeseven", cellHeight * 0.8f, FontStyle.Regular))
            {
                // 1) Draw Page Number Left (green)
                for (int i = 0; i < pageNumberLeft.Length; i++)
                {
                    int charX = startX + (i * cellWidth);
                    g.DrawString(pageNumberLeft[i].ToString(), font, Brushes.Green, charX, headerY);
                }

                // 2) Draw Service Name Background (red)
                using (Brush backgroundBrush = new SolidBrush(Color.Red))
                {
                    g.FillRectangle(backgroundBrush, serviceNameX, headerY, serviceNameBackgroundWidth, cellHeight);
                }

                // 3) Draw Service Name Text (yellow)
                for (int i = 0; i < serviceName.Length; i++)
                {
                    int charX = serviceNameX + ((i + serviceNamePadding) * cellWidth);
                    g.DrawString(serviceName[i].ToString(), font, Brushes.Yellow, charX, headerY);
                }

                // 4) Draw Page Number Right (white)
                for (int i = 0; i < pageNumberRight.Length; i++)
                {
                    int charX = pageNumberRightX + (i * cellWidth);
                    g.DrawString(pageNumberRight[i].ToString(), font, Brushes.White, charX, headerY);
                }

                // 5) Draw Timestamp (yellow, right-aligned)
                for (int i = 0; i < timestamp.Length; i++)
                {
                    int charX = timestampX + (i * cellWidth);
                    g.DrawString(timestamp[i].ToString(), font, Brushes.Yellow, charX, headerY);
                }
            }
        }
    }
}
