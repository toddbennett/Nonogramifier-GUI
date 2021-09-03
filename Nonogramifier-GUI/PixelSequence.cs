using System;
using System.Linq;

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
    }

    public enum PixelState
    {
        Empty,
        Filled,
        Undetermined
    }
}
