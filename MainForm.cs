using System;
using System.Drawing;
using System.Linq;
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
            _renderer = new Renderer();
            Text = "Telefact";
            ClientSize = new Size(800, 600);
            BackColor = Color.Black;
            DoubleBuffered = true;

            // always start on the Top Stories index page (300).
            _renderer.PageNumber = 300;

            // 1 Hz redraw for clock
            _uiTimer = new Timer { Interval = 1000 };
            _uiTimer.Tick += (_, __) => Invalidate();
            _uiTimer.Start();

            // sub‐page rotation for static story (if enabled)
            _subpageTimer = new Timer { Interval = 10000 };
            _subpageTimer.Tick += (_, __) =>
            {
                if (_renderer.PageNumber == 100 || _renderer.PageNumber == 777)
                    _renderer.NextSubpage();
                Invalidate();
            };
            _subpageTimer.Start();

            // rotate through RSS blocks every 10 s
            _pageTimer = new Timer { Interval = 10000 };
            _pageTimer.Tick += (_, __) =>
            {
                const int baseRssPage = 300;
                int current = _renderer.PageNumber;

                // which category block are we in?
                int catIndex = (current / 10) - 30; // 300→0, 310→1, etc.

                // guard: if we're completely outside, reset to first block
                if (catIndex < 0 || catIndex >= RSSPages.Categories.Count)
                {
                    _renderer.PageNumber = baseRssPage;
                    Invalidate();
                    return;
                }

                // how many stories do we actually have?
                var feed = new RSSCacheManager().GetFeed(RSSPages.Categories[catIndex]);
                int storyCount = Math.Min(feed?.Items.Count() ?? 0, 5);

                // define the range: index page + storyCount pages
                int blockStart = baseRssPage + (catIndex * 10);      // e.g. 300
                int firstPage = blockStart;                         // index
                int lastPage = blockStart + storyCount;            // last story, e.g. 305

                int next;
                if (current < firstPage)
                {
                    // if somehow before index, go to index
                    next = firstPage;
                }
                else if (current >= firstPage && current < lastPage)
                {
                    // advance through index→story1→…→storyN
                    next = current + 1;
                }
                else
                {
                    // after the last story: jump to the NEXT category’s index
                    int nextCat = (catIndex + 1) % RSSPages.Categories.Count;
                    next = baseRssPage + (nextCat * 10);
                }

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
