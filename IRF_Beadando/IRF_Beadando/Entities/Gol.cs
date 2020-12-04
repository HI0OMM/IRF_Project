using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IRF_Beadando.Entities
{
    public class Gol : Label
    {
        public Gol()
        {
            AutoSize = false;
            Width = 58;
            Height = Width;
            Paint += Gol_Paint;
        }
        private void Gol_Paint(object sender, PaintEventArgs e)
        {
            DrawImage(e.Graphics);

        }
        protected void DrawImage(Graphics g)
        {
            Image imageFile = Image.FromFile("Images/ball2.jpg");
            g.DrawImage(imageFile, new Rectangle(0, 0, Width, Height));

        }
    }
}
