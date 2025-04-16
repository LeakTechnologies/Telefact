using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;

// int totalWidth = (LeftPadding + PageWidth + RightPadding) * CellWidth;

namespace Telefact
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // Initialize the application
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Run the main form
            Application.Run(new MainForm());
        }
    }
}