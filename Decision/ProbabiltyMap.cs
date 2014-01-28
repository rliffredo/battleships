using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.Decision
{
    class ProbabilityMap
    {
        private IDictionary<CellCoords, int> _map = new Dictionary<CellCoords, int>();
        private IEnumerable<ShipInfo> _ships;
        private IEnumerable<CellCoords> _knownCells;

        public ProbabilityMap(IEnumerable<ShipInfo> ships, IEnumerable<CellCoords> knownCells)
        {
            _ships = ships;
            _knownCells = knownCells;

            for (int i = 0; i <= CellCoords.MAX; ++i)
                for (int j = 0; j <= CellCoords.MAX; ++j)
                    _map[new CellCoords(i, j)] = 0;
        }

        public List<CellCoords> GetBestCandidates()
        {
            foreach (var shipSize in ShipsToSink)
            {
                UpdateProbabilityMapForShip(shipSize);
            }
            Debug.Assert(_map.Count(c => c.Value > 0) > 0);
            var maxProb = _map.Max(c => c.Value);
            return _map.Where(c => c.Value == maxProb).Select(c => c.Key).ToList();
        }

        void UpdateProbabilityMapForShip(int shipSize)
        {
            foreach (var cell in _map.Keys.Except(_knownCells).ToList())
            {
                _map[cell] += WaysShipCanFit(cell, shipSize);
            }
        }

        int WaysShipCanFit(CellCoords cell, int shipSize)
        {
            if (shipSize == 1)
                return IsCellAlreadyHit(cell) ? 0 : 1;

            int w = 0;
            for (var i = 0; i < shipSize; ++i)
            {
                if (CanFitShip(shipSize, new CellCoords(cell.x - i, cell.y), (c, n) => c.AddHorizontal(n)))
                    w += 1;
                if (CanFitShip(shipSize, new CellCoords(cell.x, cell.y - i), (c, n) => c.AddVertical(n)))
                    w += 1;
            }
            return w;
        }

        bool CanFitShip(int shipSize, CellCoords baseCell, Func<CellCoords, int, CellCoords> offsetCell)
        {
            var cell = baseCell;
            for (var i = 0; i < shipSize; ++i)
            {
                cell = offsetCell(cell, i);
                if (IsCellAlreadyHit(cell))
                    return false;
            }
            return true;
        }

        bool IsCellAlreadyHit(CellCoords cell)
        {
            return _knownCells.Contains(cell);
        }

        private IEnumerable<int> ShipsToSink
        {
            get
            {
                return _ships
                    .Where(s => s.IsSunken == false)
                    .Select(s => s.Size)
                    .Distinct();
            }
        }
    }
}
