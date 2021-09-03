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
        private Brush brsBlack;
        private Brush brsGray;
        private Font fnt;
        private StringFormat fmtRow;
        private StringFormat fmtCol;

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
        private int longestRow;

        private Bitmap imgCols;
        private float xCols;
        private float yCols;
        private float wCols;
        private float hCols;
        private int longestCol;

        public NonogramDrawer(Graphics g)
        {
            gfx = g;
            brsBlack = new SolidBrush(Color.Black);
            brsGray = new SolidBrush(Color.LightSlateGray);
            fnt = new Font("Arial", 14);
            fmtRow = new StringFormat(StringFormatFlags.NoClip)
            {
                LineAlignment = StringAlignment.Near,
                Alignment = StringAlignment.Near
            };
            fmtCol = new StringFormat(StringFormatFlags.NoClip)
            {
                LineAlignment = StringAlignment.Near,
                Alignment = StringAlignment.Near
            };

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

        public void ResetDimensions(Graphics g)
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

        public void SetNonoDimensions(int x, int y)
        {
            xPixels = x;
            yPixels = y;
        }

        public void SetNonoPixel(int x, int y)
        {
            Graphics.FromImage(imgNono).FillRectangle(brsBlack, wNono * x / xPixels, hNono * y / yPixels, wNono / xPixels, hNono / yPixels);
        }

        public void SetNonoPixelUndetermined(int x, int y)
        {
            Graphics.FromImage(imgNono).FillRectangle(brsGray, wNono * x / xPixels, hNono * y / yPixels, wNono / xPixels, hNono / yPixels);
        }

        public void ClearNono()
        {
            Graphics.FromImage(imgNono).Clear(Color.White);
        }

        public void SetLongestRow(int max)
        {
            longestRow = max;
        }

        public void SetRow(int y, int[] row)
        {
            for (int i = 0; i < row.Length; i++)
            {
                Graphics.FromImage(imgRows).DrawString(row[i].ToString(), fnt, brsBlack, wRows - ((wRows / longestRow) * ((row.Length - i))), y * (hRows / yPixels), fmtRow);
            }
        }

        public void ClearRows()
        {
            Graphics.FromImage(imgRows).Clear(Color.White);
        }

        public void SetLongestCol(int max)
        {
            longestCol = max;
        }

        public void SetCol(int x, int[] col)
        {
            for (int i = 0; i < col.Length; i++)
            {
                Graphics.FromImage(imgCols).DrawString(col[i].ToString(), fnt, brsBlack, x * (wCols / xPixels), hCols - ((hCols / longestCol) * ((col.Length - i))), fmtCol);
            }
        }

        public void ClearColumns()
        {
            Graphics.FromImage(imgCols).Clear(Color.White);
        }

        private void DrawNonogram()
        {
            gfx.DrawImage(imgNono, xNono, yNono, wNono, hNono);
        }

        private void DrawRows()
        {
            gfx.DrawImage(imgRows, xRows, yRows, wRows, hRows);
        }

        private void DrawColumns()
        {
            gfx.DrawImage(imgCols, xCols, yCols, wCols, hCols);
        }

        public void DrawEverything()
        {
            gfx.Clear(Color.White);
            DrawNonogram();
            DrawRows();
            DrawColumns();
        }

        public void DrawToImage(Graphics bitgfx)
        {
            Graphics g = gfx;
            gfx = bitgfx;

            DrawEverything();
            gfx = g;
        }
    }
}
