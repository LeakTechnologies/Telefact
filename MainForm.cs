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
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            _renderer.Render(e.Graphics, this.ClientSize.Width, this.ClientSize.Height);
        }
    }
}
