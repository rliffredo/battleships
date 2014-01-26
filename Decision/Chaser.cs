﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.Decision
{
    class Chaser
    {
        public Chaser(IList<CellCoords> cellsToAvoid)
        {
            _cellsToAvoid = cellsToAvoid;
        }

        public IList<CellCoords> GetShots(ShipInfo ship)
        {
            return ship.Cells
                .SelectMany(c => c.GetSurroundingCells())
                .Where(c => _cellsToAvoid.Contains(c) == false)
                .ToList();
        }

        private IList<CellCoords> _cellsToAvoid = new List<CellCoords>();

    }

}
