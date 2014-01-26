using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.Decision
{
    class CellCoords
    {
        public const int MAX = 9; // 10x10 grid

        public CellCoords(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public readonly int x;
        public readonly int y;

        public Tuple<int, int> AsTuple()
        {
            return Tuple.Create(this.x, this.y);
        }

        public List<CellCoords> GetSurroundingCells()
        {
            var ret = new List<CellCoords>();
            for (int i = -1; i <= 1; ++i)
                for (int j = -1; j <= 1; ++j)
                    if (i != 0 || j != 0)
                        ret.Add(new CellCoords(this.x + i, this.y + j));
            return ret;
        }

        public static CellCoords Min() { return new CellCoords(0, 0); }
        public static CellCoords Max() { return new CellCoords(MAX, MAX); }
    }

}
