using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luck
{
    public class CoinModel
    {
        public int X;
        public int Y;
        public int Width;
        public int Height;
        public bool Removed;
        public CoinModel(int x, int y)
        {
            X = x;
            Y = y;
        }

    }
}
