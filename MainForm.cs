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
        private readonly Timer _progressTimer;

        private const int PageDurationMs = 10_000;   // 10 s per page
        private const int ProgressTickMs  = 200;      // progress bar update rate

        private int _pageElapsedMs;                   // ms elapsed on the current page

        public MainForm()
        {
            _renderer = new Renderer();
            Text = "Telefact";
            ClientSize = new Size(800, 600);
            BackColor = Color.Black;
            DoubleBuffered = true;

            // Always start on the Top Stories index page (300).
            _renderer.PageNumber = 300;

            // ── 1 Hz redraw – keeps the clock in the header live ──────────────
            _uiTimer = new Timer { Interval = 1000 };
            _uiTimer.Tick += (_, __) => Invalidate();
            _uiTimer.Start();

            // ── Sub-page rotation for static story pages (100 / 777) ──────────
            _subpageTimer = new Timer { Interval = PageDurationMs };
            _subpageTimer.Tick += (_, __) =>
            {
                if (_renderer.PageNumber == 100 || _renderer.PageNumber == 777)
                    _renderer.NextSubpage();
                Invalidate();
            };
            _subpageTimer.Start();

            // ── Broadcast loop – "Pages from Ceefax" page advance ─────────────
            _pageTimer = new Timer { Interval = PageDurationMs };
            _pageTimer.Tick += (_, __) => AdvanceBroadcastPage();
            _pageTimer.Start();

            // ── Progress bar – updates the cyan strip every 200 ms ────────────
            _progressTimer = new Timer { Interval = ProgressTickMs };
            _progressTimer.Tick += (_, __) =>
            {
                _pageElapsedMs += ProgressTickMs;
                _renderer.PageProgress = (double)_pageElapsedMs / PageDurationMs;
                Invalidate();
            };
            _progressTimer.Start();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            _renderer.Render(e.Graphics, ClientSize.Width, ClientSize.Height);
        }

        // ────────────────────────────────────────────────────────────────────────
        // Broadcast loop logic
        // Cycles: category index → story 1 → … → story N → next category index → …
        // ────────────────────────────────────────────────────────────────────────
        private void AdvanceBroadcastPage()
        {
            const int baseRssPage = 300;
            int current = _renderer.PageNumber;

            // Which category block are we in?  300→0, 310→1, …
            int catIndex = (current / 10) - 30;

            // Guard: if completely outside RSS range, reset to first block.
            if (catIndex < 0 || catIndex >= RSSPages.Categories.Count)
            {
                SetPage(baseRssPage, isNewCategory: true);
                return;
            }

            // How many stories does this category actually have right now?
            var feed = new RSSCacheManager().GetFeed(RSSPages.Categories[catIndex]);
            int storyCount = Math.Min(feed?.Items.Count() ?? 0, RSSPages.StoriesPerCategory);

            int blockStart = baseRssPage + (catIndex * 10);   // e.g. 300
            int lastStory  = blockStart + storyCount;          // e.g. 305

            int next;
            bool isNewCategory;

            if (current < blockStart || current >= lastStory)
            {
                // Past the last story (or somehow before): jump to the next category.
                int nextCat = (catIndex + 1) % RSSPages.Categories.Count;
                next = baseRssPage + (nextCat * 10);
                isNewCategory = true;
            }
            else
            {
                // Advance within the current category block.
                next = current + 1;
                isNewCategory = false;
            }

            SetPage(next, isNewCategory);
        }

        private void SetPage(int pageNumber, bool isNewCategory)
        {
            _renderer.PageNumber = pageNumber;
            _pageElapsedMs = 0;
            _renderer.PageProgress = 0.0;

            // Audio cue for the transition.
            if (isNewCategory)
                AudioManager.PlayCategoryJingle();
            else
                AudioManager.PlayPageBeep();

            Invalidate();
        }
    }
}
