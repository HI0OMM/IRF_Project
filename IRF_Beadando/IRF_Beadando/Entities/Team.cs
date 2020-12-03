using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IRF_Beadando.Entities
{
    class Team : Label
    {
        public Team()
        {
            AutoSize = false;
            Width = 58;
            Height = Width;
            Paint += Team_Paint;
        }
        private void Team_Paint(object sender, PaintEventArgs e)
        {
            DrawImage(e.Graphics);
        }

        protected void DrawImage(Graphics g)
        {
            
            g.FillRectangle(new SolidBrush(Color.White), 0, 0, Width, Height);
            g.FillRectangle(new SolidBrush(Color.Black), 0, 0, Width, 18);
            g.FillRectangle(new SolidBrush(Color.Black), 20, 0, 18, Height);
        }
    }
}
