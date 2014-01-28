using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.Decision
{
    class Chaser
    {
        public Chaser()
        {
        }

        internal IList<CellCoords> GetShots(ShipInfo currentShip, CellCoords lastHit, IEnumerable<CellCoords> cellsToAvoid)
        {
            var pivot_x = currentShip.HitCells.All(c => c.x == lastHit.x) ? lastHit.x : -1;
            var pivot_y = currentShip.HitCells.All(c => c.y == lastHit.y) ? lastHit.y : -1;

            var totalCandidates = currentShip.HitCells
                .SelectMany(c => c.GetSurroundingCells())
                .Where(c => c.x == pivot_x || c.y == pivot_y)
                .Where(c => cellsToAvoid.Contains(c) == false)
                .ToList();
            Debug.Assert(totalCandidates.Count > 0);

            var aroundLastHit = lastHit
                .GetSurroundingCells()
                .Intersect(totalCandidates)
                .ToList();

            return aroundLastHit.Count > 0 ? aroundLastHit : totalCandidates;
        }
    }

}
