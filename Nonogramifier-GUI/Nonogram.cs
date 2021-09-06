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
                if (Rows[j].Length > maxRow) maxRow = Rows[j].Length;
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
                if (Columns[i].Length > maxCol) maxCol = Columns[i].Length;
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
                if (Columns[i].Length > maxCol) maxCol = Columns[i].Length;
            }

            maxRow = 0;
            for (int j = 0; j < height; j++)
            {
                if (Rows[j].Length > maxRow) maxRow = Rows[j].Length;
            }
        }

        public void Clear(PixelState ps)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    myPixels[i, j] = ps;
                }
            }
        }

        public bool IsSolved()
        {
            for (int i = 0; i < height; i++)
            {
                if (!Rows[i].IsSolved(GetRowPS(i))) return false;
            }
            for (int i = 0; i < width; i++)
            {
                if (!Columns[i].IsSolved(GetColumnPS(i))) return false;
            }
            return true;
        }

        public void Solve()
        {
            Clear(PixelState.Undetermined);

            // Check rows
            for (int i = 0; i < height; i++)
            {
                SetRowPS(i, Rows[i].Solve(GetRowPS(i)));
            }

            // Check columns
            for (int i = 0; i < width; i++)
            {
                SetColumnPS(i, Columns[i].Solve(GetColumnPS(i)));
            }
        }

        public void SolveBruteForce()
        {
            Clear(PixelState.Empty);

            int i = 0;
            int j = 0;
            while (true)
            {
                i = 0;
                j = 0;
                while (myPixels[i, j] == PixelState.Filled)
                {
                    if (IsSolved()) return;
                    myPixels[i, j] = PixelState.Empty;
                    i++;
                    if (i >= width)
                    {
                        i = 0;
                        j++;
                    }
                    if (j >= height)
                    {
                        break;
                    }
                }
                if (j < height) myPixels[i, j] = PixelState.Filled;
                if (IsSolved()) return;
            }
        }

        public PixelState[] GetRowPS(int r)
        {
            PixelState[] ps = new PixelState[width];
            for (int i = 0; i < width; i++)
            {
                ps[i] = myPixels[i, r];
            }
            return ps;
        }

        public void SetRowPS(int r, PixelState[] ps)
        {
            for (int i = 0; i < width; i++)
            {
                myPixels[i, r] = ps[i];
            }
        }

        public PixelState[] GetColumnPS(int c)
        {
            PixelState[] ps = new PixelState[height];
            for (int i = 0; i < height; i++)
            {
                ps[i] = myPixels[c, i];
            }
            return ps;
        }

        public void SetColumnPS(int c, PixelState[] ps)
        {
            for (int i = 0; i < height; i++)
            {
                myPixels[c, i] = ps[i];
            }
        }

        public void DrawSelf(NonogramDrawer d)
        {
            d.ClearNono();
            d.SetNonoDimensions(width, height);
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (myPixels[i, j] == PixelState.Filled) d.SetNonoPixel(i, j);
                    else if (myPixels[i, j] == PixelState.Undetermined) d.SetNonoPixelUndetermined(i, j);
                }
            }

            d.ClearRows();
            d.SetLongestRow(maxRow);
            for (int j = 0; j < height; j++)
            {
                d.SetRow(j, Rows[j].GetSequence());
            }

            d.ClearColumns();
            d.SetLongestCol(maxCol);
            for (int i = 0; i < width; i++)
            {
                d.SetCol(i, Columns[i].GetSequence());
            }
        }

        public PixelSequence[] GetRows()
        {
            return Rows;
        }

        public PixelSequence[] GetColumns()
        {
            return Columns;
        }
    }
}
