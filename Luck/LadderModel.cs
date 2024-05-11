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
        public int Width = 40;
        public int Height = 150;
        public LadderModel(int x, int y)
        {
            X = x;
            Y = y;

        }
    }
}
