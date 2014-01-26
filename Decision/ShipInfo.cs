using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.Decision
{
    class ShipInfo
    {
        public const int MAX_SIZE = 4;

        public ShipInfo(int size)
        {
            _size = size;
        }

        public bool IsSunken { get { return _cells.Count == _size; } }
        public IList<CellCoords> Cells { get { return _cells; } }
        public int Size { get { return _size; } }

        private int _size;
        private IList<CellCoords> _cells = new List<CellCoords>();
    }
}
