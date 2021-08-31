using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Nonogramifier_GUI
{
    public class NonogramDrawer
    {
        private Graphics gfx;

        private Bitmap imgNono;
        private float xNono;
        private float yNono;
        private float wNono;
        private float hNono;
        private int xPixels;
        private int yPixels;

        private Bitmap imgRows;
        private float xRows;
        private float yRows;
        private float wRows;
        private float hRows;

        private Bitmap imgCols;
        private float xCols;
        private float yCols;
        private float wCols;
        private float hCols;

        public NonogramDrawer(Graphics g)
        {
            gfx = g;

            // Initiaize Nonogram image
            xNono = g.VisibleClipBounds.Right / 3;
            yNono = g.VisibleClipBounds.Bottom / 3;
            wNono = g.VisibleClipBounds.Right - xNono;
            hNono = g.VisibleClipBounds.Bottom - yNono;
            imgNono = new Bitmap((Int32)wNono, (Int32)hNono);

            // Initialize Rows image
            xRows = 0;
            yRows = yNono;
            wRows = xNono;
            hRows = hNono;
            imgRows = new Bitmap((Int32)wRows, (Int32)hRows);

            // Initialize Columns image
            xCols = xNono;
            yCols = 0;
            wCols = wNono;
            hCols = yNono;
            imgCols = new Bitmap((Int32)wCols, (Int32)hCols);
        }

        public void setNonoDimensions(int x, int y)
        {
            xPixels = x;
            yPixels = y;
        }

        private void drawNonogram()
        {
            gfx.DrawImage(imgNono, xNono, yNono, wNono, hNono);
        }

        private void drawRows()
        {
            gfx.DrawImage(imgRows, xRows, yRows, wRows, hRows);
        }

        private void drawColumns()
        {
            gfx.DrawImage(imgCols, xCols, yCols, wCols, hCols);
        }

        public void DrawEverything()
        {
            drawNonogram();
            drawRows();
            drawColumns();
        }
    }
}
