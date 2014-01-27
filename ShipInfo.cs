using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships
{
    class ShipInfo
    {
        public const int MAX_SIZE = 4;

        public ShipInfo(int size)
        {
            for (var i = 0; i < size; i++)
                _positionCells.Add(null);
        }

        public static IList<ShipInfo> CreateGameShips()
        {
            var ships = new List<ShipInfo>();
            for (int i = 1; i <= ShipInfo.MAX_SIZE; ++i)
                for (int j = i; j <= 4; ++j)
                    ships.Add(new ShipInfo(i));
            return ships;
        }

        public bool IsSunken { get { return HitCells.Count == Size; } }
        public IList<CellCoords> HitCells { get { return _hitCells; } }
        public IList<CellCoords> PositionCells { get { return _positionCells; } }
        public int Size { get { return _positionCells.Count; } }

        private IList<CellCoords> _positionCells = new List<CellCoords>();
        private IList<CellCoords> _hitCells = new List<CellCoords>();
    }
}
