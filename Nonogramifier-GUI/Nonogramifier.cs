using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Nonogramifier_GUI
{
    public partial class Nonogramifier : Form
    {
        private Bitmap img;
        private Nonogram nono;
        private NonogramDrawer d;

        public Nonogramifier()
        {
            InitializeComponent();
            d = new NonogramDrawer(picBox.CreateGraphics());
        }

        private void CreateNonogram()
        {
            int width = img.Width;
            int height = img.Height;
            PixelState[,] pixels = new PixelState[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (img.GetPixel(i, j).B == 0)
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

        private void DrawNonogram()
        {
            nono.DrawSelf(d);
            d.DrawEverything();
        }

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog
            {
                Filter = "bmp files (*.bmp)|*.bmp"
            };

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                img = new Bitmap(dlg.OpenFile());
            }

            dlg.Dispose();

            CreateNonogram();

            DrawNonogram();
        }

        private void Nonogramifier_Resize(object sender, EventArgs e)
        {
            d.ResetDimensions(picBox.CreateGraphics());
            nono.DrawSelf(d);
            d.DrawEverything();
        }

        private void solveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Nonogram n = new Nonogram(nono.GetRows(), nono.GetColumns());
            nono = n;
            nono.Solve();
            d.ResetDimensions(picBox.CreateGraphics());
            nono.DrawSelf(d);
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
