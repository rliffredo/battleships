using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships
{
    class CellCoords : IEquatable<CellCoords>
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

        public IList<CellCoords> GetSurroundingCells()
        {
            var ret = new List<CellCoords>();
            for (int i = -1; i <= 1; ++i)
                for (int j = -1; j <= 1; ++j)
                    if (i != 0 || j != 0)
                    {
                        var cell = new CellCoords(this.x + i, this.y + j);
                        if (cell.x >= 0 && cell.y >= 0 && cell.x <= MAX && cell.y <= MAX)
                            ret.Add(cell);
                    }
            return ret;
        }

        public static CellCoords Min() { return new CellCoords(0, 0); }
        public static CellCoords Max() { return new CellCoords(MAX, MAX); }

        public CellCoords AddHorizontal(int n) { return new CellCoords(this.x + n, this.y); }
        public CellCoords AddVertical(int n) { return new CellCoords(this.x, this.y + n); }

        public bool Equals(CellCoords other)
        {
            if (other == null)
                return false;
            return this.x == other.x && this.y == other.y;
        }

        public override bool Equals(Object other)
        {
            if (other == null)
                return false;
            var cell = other as CellCoords;
            if (cell == null)
                return false;
            return Equals(cell);
        }

        public override int GetHashCode()
        {
            return this.x + this.y * CellCoords.MAX;
        }
    }

}
