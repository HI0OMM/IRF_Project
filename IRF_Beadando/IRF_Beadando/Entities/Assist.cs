using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IRF_Beadando.Entities
{
    class Assist : Label
    {
        public Assist()
        {
            AutoSize = false;
            Width = 58;
            Height = Width;
            Paint += Assist_Paint;
        }
        private void Assist_Paint(object sender, PaintEventArgs e)
        {
            DrawImage(e.Graphics);
        }

        protected void DrawImage(Graphics g)
        {
            Pen MyPen = new Pen(Color.Black,10);
            g.FillRectangle(new SolidBrush(Color.White), 0, 0, Width, Height);
            g.DrawLine(MyPen, 0, 58, 29, 0);
            g.DrawLine(MyPen, 58, 58, 29, 0);
            g.DrawLine(MyPen, 0, 36, 58, 36);

        }
    }
}
