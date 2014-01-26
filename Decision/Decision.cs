using System;
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
            // TODO
            //if (result == ShotResult.Hit)
            //    MarkCurrentShipAsHit(x, y);
            //if (result == ShotResult.HitAndSunk)
            //    MarkCurrentShipAsSunk(x, y);
            //if (result == ShotResult.Miss)
            //    boardState[x, y] = CellStates.Water;
        }

        public Decision()
        {
            CreateInitialShips();
        }

        private void CreateInitialShips()
        {
            for (int i = 1; i <= ShipInfo.MAX_SIZE; ++i)
                for (int j = i; j <= 4; ++j)
                    _ships.Add(new ShipInfo(i));
        }

        private bool IsChasing()
        {
            return _currentShip != null;
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

        private ShipInfo _currentShip = null;
        private IList<ShipInfo> _ships = new List<ShipInfo>();
        private IList<CellCoords> _knownCells = new List<CellCoords>();

    }

}
