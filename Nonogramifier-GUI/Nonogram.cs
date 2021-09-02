using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nonogramifier_GUI
{
    public class Nonogram
    {
        private PixelState[,] myPixels;
        private int width;
        private int height;
        private PixelSequence[] Rows;
        private PixelSequence[] Columns;
        private int maxRow = 0;
        private int maxCol = 0;

        public Nonogram(PixelState[,] Pixels)
        {
            this.myPixels = Pixels;
            width = myPixels.GetLength(0);
            height = myPixels.GetLength(1);
            Rows = new PixelSequence[height];
            for (int j = 0; j < height; j++)
            {
                PixelState[] sequence = new PixelState[width];
                for (int i = 0; i < width; i++)
                {
                    sequence[i] = myPixels[i, j];
                }
                Rows[j] = new PixelSequence(sequence);
                if (Rows[j].length > maxRow) maxRow = Rows[j].length;
            }

            Columns = new PixelSequence[width];
            for (int i = 0; i < width; i++)
            {
                PixelState[] sequence = new PixelState[height];
                for (int j = 0; j < height; j++)
                {
                    sequence[j] = myPixels[i, j];
                }
                Columns[i] = new PixelSequence(sequence);
                if (Columns[i].length > maxCol) maxCol = Columns[i].length;
            }
        }

        public Nonogram(PixelSequence[] r, PixelSequence[] c)
        {
            Rows = r;
            Columns = c;
            width = c.Length;
            height = r.Length;
            myPixels = new PixelState[width, height];

            maxCol = 0;
            for (int i = 0; i < width; i++)
            {
                if (Columns[i].length > maxCol) maxCol = Columns[i].length;
            }

            maxRow = 0;
            for (int j = 0; j < height; j++)
            {
                if(Rows[j].length > maxRow) maxRow = Rows[j].length;
            }

            Solve();
        }

        public void Solve()
        {
            // Clear pixel board
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    myPixels[i, j] = PixelState.Undetermined;
                }
            }

            // Check rows
            for (int i = 0; i < height; i++)
            {
                setRowPS(i, Rows[i].Solve(getRowPS(i)));
            }

            // Check columns
            for (int i = 0; i < width; i++)
            {
                setColumnPS(i, Columns[i].Solve(getColumnPS(i)));
            }
        }

        public PixelState[] getRowPS(int r)
        {
            PixelState[] ps = new PixelState[width];
            for (int i = 0; i < width; i++)
            {
                ps[i] = myPixels[i, r];
            }
            return ps;
        }

        public void setRowPS(int r, PixelState[] ps)
        {
            for (int i = 0; i < width; i++)
            {
                myPixels[i, r] = ps[i];
            }
        }

        public PixelState[] getColumnPS(int c)
        {
            PixelState[] ps = new PixelState[height];
            for (int i = 0; i < height; i++)
            {
                ps[i] = myPixels[c, i];
            }
            return ps;
        }

        public void setColumnPS(int c, PixelState[] ps)
        {
            for (int i = 0; i < height; i++)
            {
                myPixels[c, i] = ps[i];
            }
        }

        public void DrawToImage(NonogramDrawer d)
        {
            d.clearNono();
            d.setNonoDimensions(width, height);
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (myPixels[i, j] == PixelState.Filled) d.setNonoPixel(i, j);
                    else if (myPixels[i, j] == PixelState.Undetermined) d.setNonoPixelUndetermined(i, j);
                }
            }

            d.clearRows();
            d.setLongestRow(maxRow);
            for (int j = 0; j < height; j++)
            {
                d.setRow(j, Rows[j].getSequence());
            }

            d.clearColumns();
            d.setLongestCol(maxCol);
            for (int i = 0; i < width; i++)
            {
                d.setCol(i, Columns[i].getSequence());
            }
        }

        public PixelSequence[] getRows()
        {
            return Rows;
        }

        public PixelSequence[] getColumns()
        {
            return Columns;
        }
    }
}
