using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.Decision
{

    class Decision: IDecision
    {
        public CellCoords CellToAttack()
        {
            if (IsChasing())
                return RandomShot(ChaseShip());

            var unknownPatch = LargestUnknownPatch();
            if (unknownPatch.Count > 35)
                return RandomShot(unknownPatch);

            var probabilityMap = CalculateProbabilityMap();
            return RandomShot(probabilityMap);
        }

        public void UpdateWithFeedback(int x, int y, ShotResult result)
        {
            if (result == ShotResult.Hit)
                MarkShipAsHit(x, y);
            if (result == ShotResult.HitAndSunk)
                MarkShipAsSunk(x, y);
            if (result == ShotResult.Miss)
                MarkKnown(x, y);
        }

        private void MarkKnown(int x, int y)
        {
            _knownCells.Add(new CellCoords(x, y));
        }

        private void MarkShipAsHit(int x, int y)
        {
            MarkKnown(x, y);

            _lastHit = new CellCoords(x, y);
            _currentShip.HitCells.Add(_lastHit);
        }

        private void MarkShipAsSunk(int x, int y)
        {
            MarkShipAsHit(x, y);

            var ship = _ships.First(s => s.Size == _currentShip.HitCells.Count && !s.IsSunken);
            foreach (var cell in _currentShip.HitCells)
            {
                ship.HitCells.Add(cell);
                foreach (var adjCell in cell.GetSurroundingCells())
                {
                    _knownCells.Add(adjCell);
                }
                Debug.Assert(_knownCells.Count <= new BoardArea(CellCoords.Min(), CellCoords.Max()).Area);
            }

            Debug.Assert(_knownCells.IsProperSupersetOf(_ships.SelectMany(s => s.HitCells)));
            _lastHit = null;
            _currentShip.HitCells.Clear();
        }

        private bool IsChasing()
        {
            return _lastHit != null;
        }

        private IList<CellCoords> ChaseShip()
        {
            var chaser = new Chaser();
            return chaser.GetShots(_currentShip, _lastHit, _knownCells);
        }

        private CellCoords RandomShot(IList<CellCoords> candidates)
        {
            var r = new Random();
            var pos = r.Next(candidates.Count);
            return candidates[pos];

        }

        private List<CellCoords> LargestUnknownPatch()
        {
            if (_knownCells.Count > 15)
                return new BoardArea(CellCoords.Min(), CellCoords.Min()).AllCells;
            var entireBoard = new BoardArea(CellCoords.Min(), CellCoords.Max());
            var res = entireBoard.FindLargestWithout(_knownCells, 0);
            return res.AllCells;
        }

        private List<CellCoords> CalculateProbabilityMap()
        {
            var map = new ProbabilityMap(_ships, _knownCells);
            var candidates = map.GetBestCandidates();
            Debug.Assert(candidates.Intersect(_knownCells).Count() == 0);
            return candidates;
        }

        private ShipInfo _currentShip = new ShipInfo(0);
        private IList<ShipInfo> _ships = ShipInfo.CreateGameShips();
        private ISet<CellCoords> _knownCells = new HashSet<CellCoords>();
        private CellCoords _lastHit;

    }

}
