using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.Decision
{
    class Chaser
    {
        public Chaser(IEnumerable<CellCoords> cellsToAvoid)
        {
            _cellsToAvoid = cellsToAvoid;
        }

        public IList<CellCoords> GetShots(ShipInfo ship)
        {
            return ship.HitCells
                .SelectMany(c => c.GetSurroundingCells())
                .Where(c => _cellsToAvoid.Contains(c) == false)
                .ToList();
        }

        private IEnumerable<CellCoords> _cellsToAvoid;

    }

}
