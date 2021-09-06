using System;
using System.Linq;
using System.Collections.Generic;

namespace Nonogramifier_GUI
{
    public class PixelSequence
    {
        private int[] Sequence = { };
        public PixelSequence(PixelState[] Pixels)
        {
            int c = 0;
            foreach (PixelState p in Pixels)
            {
                switch (p)
                {
                    case PixelState.Empty:
                        if (c != 0)
                        {
                            Sequence = Sequence.Append(c).ToArray();
                            c = 0;
                        }
                        break;
                    case PixelState.Filled:
                        c++;
                        break;
                    case PixelState.Undetermined:
                        throw new Exception();
                }
            }
            if (c != 0)
            {
                Sequence = Sequence.Append(c).ToArray();
            }
        }

        public int[] GetSequence() => Sequence;

        public int Length { get { return Sequence.Length; } }

        public PixelState[] Solve(PixelState[] ps)
        {
            if (Sequence.Length == 0)
            {
                for (int i = 0; i < ps.Length; i++)
                {
                    ps[i] = PixelState.Empty;
                }
            }
            else if (NumFilled() == ps.Length)
            {
                for (int i = 0; i < ps.Length; i++)
                {
                    ps[i] = PixelState.Filled;
                }
            }

            if (Longest() > (ps.Length - SequenceSum()))
            {
                int position = 0;
                foreach (int s in Sequence)
                {
                    int skip = (ps.Length - SequenceSum());

                    if (s > skip)
                    {
                        for (int i = position + skip; i < (s + position); i++)
                        {
                            ps[i] = PixelState.Filled;
                        }
                    }

                    position += s + 1;
                }
            }

            if (IsComplete(ps))
            {
                for (int i = 0; i < ps.Length; i++)
                {
                    if (ps[i] == PixelState.Undetermined) ps[i] = PixelState.Empty;
                }
            }
            return ps;
        }

        private int SequenceSum()
        {
            int sum = 0;
            foreach (int i in Sequence)
            {
                sum += i;
            }
            sum += (Sequence.Length - 1);
            return sum;
        }

        private int NumFilled()
        {
            int sum = 0;
            foreach (int i in Sequence)
            {
                sum += i;
            }
            return sum;
        }

        private int NumFilledPS(PixelState[] ps)
        {
            int sum = 0;
            foreach (PixelState p in ps)
            {
                if (p == PixelState.Filled)
                {
                    sum++;
                }
            }
            return sum;
        }

        private int NumEmptyPS(PixelState[] ps)
        {
            int sum = 0;
            foreach (PixelState p in ps)
            {
                if (p == PixelState.Empty)
                {
                    sum++;
                }
            }
            return sum;
        }

        private int Longest()
        {
            int l = 0;
            foreach (int i in Sequence)
            {
                l = i > l ? i : l;
            }
            return l;
        }

        private bool IsComplete(PixelState[] ps)
        {
            int psSum = 0;
            foreach (PixelState p in ps)
            {
                if (p == PixelState.Filled)
                {
                    psSum++;
                }
            }

            int sum = 0;
            foreach (int i in Sequence)
            {
                sum += i;
            }

            return (psSum == sum);
        }

        private bool IsImpossible(PixelState[] ps)
        {
            if (Sequence.Length == 0) return false;
            int s = Sequence[0];
            int i = 0;
            while (ps[i] == PixelState.Empty) i++;
            while (s > 0)
            {
                if (i >= ps.Length) return true;
                if (ps[i] == PixelState.Empty)
                {
                    s = Sequence[0];
                    while (ps[i] == PixelState.Empty)
                    {
                        i++;
                        if (i >= ps.Length) return true;
                    }
                    continue;
                }
                i++;
                s--;
            }

            s = Sequence[Sequence.Length - 1];
            i = ps.Length - 1;
            while (ps[i] == PixelState.Empty) i--;
            while (s > 0)
            {
                if (i < 0) return true;
                if (ps[i] == PixelState.Empty)
                {
                    s = Sequence[Sequence.Length - 1];
                    while (ps[i] == PixelState.Empty)
                    {
                        i--;
                        if (i < 0) return true;
                    }
                    continue;
                }
                i--;
                s--;
            }
            return false;
        }

        public bool IsSolved(PixelState[] ps)
        {
            int i = 0;
            foreach (int s in Sequence)
            {
                if (i >= ps.Length) return false;
                while (i < ps.Length && ps[i] == PixelState.Empty)
                {
                    i++;
                }
                int j = s;
                while (i < ps.Length && ps[i] == PixelState.Filled)
                {
                    i++;
                    j--;
                }
                if (j != 0) return false;
                if (i < ps.Length && ps[i] != PixelState.Empty) return false;
                i++;
            }
            while (i < ps.Length)
            {
                if (ps[i] != PixelState.Empty) return false;
                i++;
            }
            return true;
        }

        public PixelState[] SolveBrute(PixelState[] ps)
        {
            List<PixelState[]> combos = FindCombinations(ps);
            for (int i = 0; i < ps.Length; i++)
            {
                if (ps[i] == PixelState.Undetermined)
                {
                    bool e = false;
                    bool f = false;
                    foreach (PixelState[] c in combos)
                    {
                        if (c[i] == PixelState.Empty)
                        {
                            e = true;
                        }
                        if (c[i] == PixelState.Filled)
                        {
                            f = true;
                        }
                    }

                    if (e && f)
                    {
                        ps[i] = PixelState.Undetermined;
                    }
                    else if (e)
                    {
                        ps[i] = PixelState.Empty;
                    }
                    else if (f)
                    {
                        ps[i] = PixelState.Filled;
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
            }
            return ps;
        }

        public List<PixelState[]> FindCombinations(PixelState[] ps)
        {
            List<PixelState[]> combos = new List<PixelState[]>();
            if (NumFilledPS(ps) > NumFilled()) return combos;
            if (NumEmptyPS(ps) > ps.Length - NumFilled()) return combos;
            if (IsImpossible(ps)) return combos;
            int i = 0;
            while (i < ps.Length && ps[i] != PixelState.Undetermined) i++;

            if (i >= ps.Length)
            {
                if (IsSolved(ps))
                {
                    combos.Add((PixelState[]) ps.Clone());
                }
                return combos;
            }

            ps[i] = PixelState.Filled;
            combos.AddRange(FindCombinations(ps));
            ps[i] = PixelState.Empty;
            combos.AddRange(FindCombinations(ps));
            ps[i] = PixelState.Undetermined;

            return combos;
        }
    }

    public enum PixelState
    {
        Empty,
        Filled,
        Undetermined,
    }
}
