using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nonogramifier_GUI
{
    public class Nonogram
    {
        private bool[,] myPixels;
        private int width;
        private int height;
        private PixelSequence[] Rows;
        private PixelSequence[] Columns;
        private int maxRow = 0;
        private int maxCol = 0;

        public Nonogram(bool[,] Pixels)
        {
            this.myPixels = Pixels;
            width = myPixels.GetLength(1);
            height = myPixels.GetLength(0);
            Rows = new PixelSequence[height];
            for (int j = 0; j < height; j++)
            {
                bool[] sequence = new bool[width];
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
                bool[] sequence = new bool[height];
                for (int j = 0; j < width; j++)
                {
                    sequence[j] = myPixels[i, j];
                }
                Columns[i] = new PixelSequence(sequence);
                if (Columns[i].length > maxCol) maxCol = Columns[i].length;
            }

        }

        public int Width { get; }
        public int Height { get; }

        public int RowsWidth { get { return maxRow; } }
        public int ColsHeight {  get { return maxCol; } }
    }
}
