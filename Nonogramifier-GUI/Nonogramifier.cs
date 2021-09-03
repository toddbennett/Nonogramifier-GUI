using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Nonogramifier_GUI
{
    public partial class Nonogramifier : Form
    {
        private Bitmap image;
        private Nonogram nono = null;
        private NonogramDrawer d;

        public Nonogramifier()
        {
            InitializeComponent();
            d = new NonogramDrawer(picBox.CreateGraphics());
        }

        private void createNonogram()
        {
            int width = image.Width;
            int height = image.Height;
            PixelState[,] pixels = new PixelState[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (image.GetPixel(i, j).B == 0)
                    {
                        pixels[i, j] = PixelState.Filled;
                    }
                    else
                    {
                        pixels[i, j] = PixelState.Empty;
                    }
                }
            }

            nono = new Nonogram(pixels);
        }

        private void drawNonogram()
        {

            /*
            Graphics gfx = picBox.CreateGraphics();
            gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            gfx.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            gfx.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
            Brush brsBlack = new SolidBrush(Color.Black);
            Font fnt = new Font("Arial", 14);

            float w = gfx.VisibleClipBounds.Right;
            float h = gfx.VisibleClipBounds.Bottom;

            int x = nono.Width + nono.RowsWidth;
            int y = nono.Height + nono.ColsHeight;

            gfx.Clear(Color.White);

            // Draw the Column numbers
            StringFormat fmt = new StringFormat(StringFormatFlags.DirectionVertical);
            fmt.LineAlignment = StringAlignment.Near;
            fmt.Alignment = StringAlignment.Near;
            gfx.DrawString("1 1", fnt, brsBlack, w / 3, 0, fmt);

            // Draw the Row numbers
            gfx.DrawString("1 1", fnt, brsBlack, 0, h / 3);

            // Draw the Nonogram itself
            gfx.DrawImage(image, w / 3, h / 3, h * 2 / 3, h * 2 / 3);
            */

            nono.DrawToImage(d);
            d.DrawEverything();
        }

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "bmp files (*.bmp)|*.bmp";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                image = new Bitmap(dlg.OpenFile());
            }

            dlg.Dispose();

            createNonogram();

            drawNonogram();
        }

        private void Nonogramifier_Resize(object sender, EventArgs e)
        {
            d.ResetDimensions(picBox.CreateGraphics());
            nono.DrawToImage(d);
            d.DrawEverything();
        }

        private void solveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Nonogram n = new Nonogram(nono.getRows(), nono.getColumns());
            nono = n;
            nono.Solve();
            d.ResetDimensions(picBox.CreateGraphics());
            nono.DrawToImage(d);
            d.DrawEverything();
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            Stream s;
            dlg.Filter = "bmp files (*.bmp)|*.bmp";
            
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if ((s = dlg.OpenFile()) != null)
                {
                    Bitmap bmp = new Bitmap(picBox.Width, picBox.Height);
                    d.DrawToImage(Graphics.FromImage(bmp));
                    bmp.Save(s, System.Drawing.Imaging.ImageFormat.Bmp);
                    s.Close();
                }
            }
        }
    }
}
