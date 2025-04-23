using System;
using System.Drawing;
using System.Windows.Forms;

namespace Telefact
{
    public class MainForm : Form
    {
        private readonly Renderer _renderer;
        private readonly Timer _uiTimer;
        private readonly Timer _subpageTimer;
        private readonly Timer _pageTimer;

        public MainForm()
        {
            _renderer = new Renderer
            {
                // start on the first RSS index page
                PageNumber = 300
            };

            Text = "Telefact";
            ClientSize = new Size(800, 600);
            BackColor = Color.Black;
            DoubleBuffered = true;

            // 1 Hz redraw (for clock)
            _uiTimer = new Timer { Interval = 1000 };
            _uiTimer.Tick += (_, __) => Invalidate();
            _uiTimer.Start();

            // sub-page rotation (for static story debug page)
            _subpageTimer = new Timer { Interval = 10000 };
            _subpageTimer.Tick += (_, __) =>
            {
                if (_renderer.PageNumber == 100 || _renderer.PageNumber == 777)
                    _renderer.NextSubpage();
                Invalidate();
            };
            _subpageTimer.Start();

            // rotate through RSS pages every 10 s
            _pageTimer = new Timer { Interval = 10000 };
            _pageTimer.Tick += (_, __) =>
            {
                int first = 300;
                int last = 300 + RSSPages.Categories.Count * RSSPages.PagesPerCategory - 1;
                int next = _renderer.PageNumber < first || _renderer.PageNumber >= last
                              ? first
                              : _renderer.PageNumber + 1;
                _renderer.PageNumber = next;
                Invalidate();
            };
            _pageTimer.Start();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            _renderer.Render(e.Graphics, ClientSize.Width, ClientSize.Height);
        }
    }
}
