using System;
using System.Drawing;

namespace Telefact
{
    public class Renderer
    {
        private readonly TeletextHeader _header = new TeletextHeader();
        private readonly TeletextFooter _footer = new TeletextFooter();

        public void Render(Graphics g, int clientWidth, int clientHeight)
        {
            if (g == null) throw new ArgumentNullException(nameof(g));

            // Render header
            _header.Render(g, clientWidth);

            // Render footer
            _footer.Render(g, clientWidth, clientHeight);
        }
    }
}
