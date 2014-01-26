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
    }

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
            for (int i = 1; i <= 4; ++i)
                for (int j = i; j <= 4; ++j)
                    _ships.Add(new ShipInfo(i));
        }

        private bool IsChasing()
        {
            // TODO
            return false;
        }

        private List<CellCoords> ChaseShip()
        {
            // TODO
            return new List<CellCoords>();
        }

        private CellCoords RandomShot(IList<CellCoords> candidates)
        {
            var r = new Random();
            var pos = r.Next(candidates.Count);
            return candidates[pos];

        }

        private List<CellCoords> LargestUnknownPatch()
        {
            var entireBoard = new BoardArea(new CellCoords(0, 0), new CellCoords(CellCoords.MAX, CellCoords.MAX));
            var res = entireBoard.FindLargestWithout(_knownCells);
            return res.AllCells;
        }

        private List<CellCoords> CalculateProbabilityMap()
        {
            var map = new ProbabilityMap(_ships, _knownCells);
            return map.GetBestCandidates();
        }

        private IList<ShipInfo> _ships = new List<ShipInfo>();
        private IList<CellCoords> _knownCells = new List<CellCoords>();

    }
    class ShipInfo
    {
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
