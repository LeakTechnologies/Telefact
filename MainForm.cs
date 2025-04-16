using System;
using System.Drawing;
using System.Windows.Forms;

namespace Telefact
{
    public class MainForm : Form
    {
        private readonly Renderer _renderer;

        public MainForm()
        {
            _renderer = new Renderer();
            this.Text = "Telefact";
            this.ClientSize = new Size(800, 600);
            this.BackColor = Color.Black;
            this.DoubleBuffered = true;

            // Timer for updating the UI every second (e.g., for timestamp update).
            Timer uiTimer = new Timer();
            uiTimer.Interval = 1000; // 1 second.
            uiTimer.Tick += (s, e) => { this.Invalidate(); };
            uiTimer.Start();

            // New timer for rotating subpages every 10 seconds.
            Timer subpageTimer = new Timer();
            subpageTimer.Interval = 10000; // 10 seconds.
            subpageTimer.Tick += (s, e) =>
            {
                _renderer.NextSubpage();
                this.Invalidate();
            };
            subpageTimer.Start();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            _renderer.Render(e.Graphics, this.ClientSize.Width, this.ClientSize.Height);
        }
    }
}
