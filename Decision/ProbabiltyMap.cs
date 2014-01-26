using System;
using System.Collections.Generic;
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

            for (int i = 0; i < CellCoords.MAX; ++i)
                for (int j = 0; j < CellCoords.MAX; ++j)
                    _map[new CellCoords(i, j)] = 0;
        }

        public List<CellCoords> GetBestCandidates()
        {
            foreach (var shipSize in ShipsToSink)
            {
                UpdateProbabilityMapForShip(shipSize);
            }
            var maxProb = _map.Max(c => c.Value);
            return _map.Where(c => c.Value == maxProb).Select(c => c.Key).ToList();
        }

        void UpdateProbabilityMapForShip(int shipSize)
        {
            foreach (var cell in _map.Keys)
            {
                _map[cell] += WaysShipCanFit(cell, shipSize);
            }
        }

        int WaysShipCanFit(CellCoords cell, int shipSize)
        {
            if (shipSize == 1)
                return HasAdjacents(cell) ? 0 : 1;

            int w = 0;
            for (var i = 0; i < shipSize; ++i)
            {
                w += CanFitShip(shipSize, new CellCoords(cell.x - i, cell.y), (c, n) => new CellCoords(c.x + n, c.y)) ? 1 : 0;
                w += CanFitShip(shipSize, new CellCoords(cell.x, cell.y - i), (c, n) => new CellCoords(c.x, c.y + n)) ? 1 : 0;
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
                if (HasAdjacents(cell))
                    return false;
            }
            return true;
        }

        bool IsCellAlreadyHit(CellCoords cell)
        {
            return _knownCells.Contains(cell);
        }

        bool HasAdjacents(CellCoords cell)
        {
            var cellsToCheck = cell.GetSurroundingCells();
            return _ships.Any(ship => cellsToCheck.Intersect(ship.Cells).Count() > 0);
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
