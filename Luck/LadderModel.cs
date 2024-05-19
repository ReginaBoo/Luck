using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luck
{
    public class LadderModel
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width;
        public int Height;
        public LadderModel(int x, int y, int h, int w)
        {
            X = x;
            Y = y;
            Height = h;
            Width = w;
        }
    }
}
