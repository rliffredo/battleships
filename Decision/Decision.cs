﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.Decision
{

    class Decision: IDecision
    {
        public Tuple<int, int> CellToAttack()
        {
            if (IsChasing())
                return RandomShot(ChaseShip()).AsTuple();

            var unknownPatch = LargestUnknownPatch();
            if (unknownPatch.Count > 20)
                return RandomShot(unknownPatch).AsTuple();

            var probabilityMap = CalculateProbabilityMap();
            return RandomShot(probabilityMap).AsTuple();
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

            var cell = new CellCoords(x, y);
            _currentShip.HitCells.Add(cell);

            throw new NotImplementedException();
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
            }

            _currentShip.HitCells.Clear();
        }

        private bool IsChasing()
        {
            return _currentShip.HitCells.Count > 0;
        }

        private IList<CellCoords> ChaseShip()
        {
            var chaser = new Chaser(_knownCells);
            return chaser.GetShots(_currentShip);
        }

        private CellCoords RandomShot(IList<CellCoords> candidates)
        {
            var r = new Random();
            var pos = r.Next(candidates.Count);
            return candidates[pos];

        }

        private List<CellCoords> LargestUnknownPatch()
        {
            var entireBoard = new BoardArea(CellCoords.Min(), CellCoords.Max());
            var res = entireBoard.FindLargestWithout(_knownCells);
            return res.AllCells;
        }

        private List<CellCoords> CalculateProbabilityMap()
        {
            var map = new ProbabilityMap(_ships, _knownCells);
            return map.GetBestCandidates();
        }

        private ShipInfo _currentShip = new ShipInfo(0);
        private IList<ShipInfo> _ships = ShipInfo.CreateGameShips();
        private ISet<CellCoords> _knownCells = new HashSet<CellCoords>();

    }

}