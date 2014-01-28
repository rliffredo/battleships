using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Battleships.Decision
{
    class BoardArea
    {
        public BoardArea(CellCoords topLeft, CellCoords bottomRight)
        {
            _top = topLeft.y;
            _left = topLeft.x;
            _bottom = bottomRight.y;
            _right = bottomRight.x;
            for (var x = _left; x <= _right; ++x)
                for (var y = _top; y <= _bottom; ++y)
                    _allCells.Add(new CellCoords(x, y));
        }

        private readonly int _top;
        private readonly int _bottom;
        private readonly int _left;
        private readonly int _right;
        private readonly List<CellCoords> _allCells = new List<CellCoords>();

        public CellCoords TopLeft { get { return new CellCoords(_left, _top); } }
        public CellCoords BottomRight { get { return new CellCoords(_right, _bottom); } }
        public int Area { get { return _allCells.Count; } }
        public List<CellCoords> AllCells { get { return _allCells; } }
        public List<BoardArea> Split(CellCoords pivot)
        {
            Debug.Assert(this.Contains(pivot));
            var ret = new List<BoardArea>();
            // Left
            if (pivot.x > _left)
                ret.Add(new BoardArea(TopLeft, new CellCoords(pivot.x - 1, _bottom)));
            // Above
            if (pivot.y > _top)
                ret.Add(new BoardArea(TopLeft, new CellCoords(_right, pivot.y - 1)));
            // Right
            if (pivot.x < _right)
                ret.Add(new BoardArea(new CellCoords(pivot.x + 1, _top), BottomRight));
            // Below
            if (pivot.y < _bottom)
                ret.Add(new BoardArea(new CellCoords(_left, pivot.y + 1), BottomRight));
            return ret;
        }
        public bool Contains(CellCoords cell)
        {
            return (cell.x >= _left && cell.x <= _right) && (cell.y >= _top && cell.y <= _bottom);
        }

        public BoardArea FindLargestWithout(IEnumerable<CellCoords> cells)
        {
            if (Area == 1)
                return this;

            var cellList = cells.ToList();
            if (cellList.Count == 0)
                return this;

            var cell = cellList.FirstOrDefault(c => this.Contains(c));
            if (cell == null)
                return this;

            var slices = this.Split(cell);
            var reducedCells = cellList.SkipWhile(c => c == cell).ToList();
            var largestPatches = slices.Select(s => s.FindLargestWithout(reducedCells));
            var maxArea = largestPatches.Max(p => p.Area);
            Debug.Assert(maxArea < Area);
            return largestPatches.First(p => p.Area == maxArea);
        }

    }

}
