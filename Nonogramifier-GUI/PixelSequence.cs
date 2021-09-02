using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nonogramifier_GUI
{
    public class PixelSequence
    {
        private int[] Sequence = { };
        public PixelSequence(bool[] Pixels)
        {
            int c = 0;
            foreach (bool b in Pixels)
            {
                if (!b)
                {
                    if (c != 0)
                    {
                        Sequence = Sequence.Append(c).ToArray();
                        c = 0;
                    }
                }
                else
                {
                    c++;
                }
            }
            if (c != 0)
            {
                Sequence = Sequence.Append(c).ToArray();
            }
        }

        public int[] getSequence() => Sequence;

        public int length { get { return Sequence.Length; } }

    }

   
}
