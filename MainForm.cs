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

            // Setup a timer that invalidates the form every second so the timestamp updates dynamically.
            Timer timer = new Timer();
            timer.Interval = 1000; // 1 second interval
            timer.Tick += (s, e) => { this.Invalidate(); };
            timer.Start();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            _renderer.Render(e.Graphics, this.ClientSize.Width, this.ClientSize.Height);
        }
    }
}
