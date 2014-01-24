using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Battleships
{
    class Decision
    {
        public Tuple<int, int> CellToAttack()
        {
            ResetProbabilityMap();
            CalculateProbabilityMap();
            return PickBestCell();
        }

        public void UpdateWithFeedback(int x, int y, ShotResult result)
        {
            if (ShotResult.)
        }

        private Tuple<int, int> PickBestCell()
        {
            var r = new Random();
            return Tuple.Create(r.Next(10), r.Next(10));
        }

        private void CalculateProbabilityMap()
        {
            for (var shipType=0; shipType<4; shipType++)
            {
                if (shipsToSink[shipType] == 0)
                    continue;
                UpdateProbabilityMapForShip(shipType);
            }
        }

        private void ResetProbabilityMap()
        {
            for (var i = 0; i < 10; i++)
            {
                for (var j = 0; j < 10; j++)
                {
                    probabilityMap[i, j] = 0;
                }
            }
        }

        private void UpdateProbabilityMapForShip(int shipType)
        {
            for (var i = 0; i < 10; i++)
            {
                for (var j = 0; j < 10; j++)
                {
                    var n = WaysShipCanUseCell(shipType, i, j);
                    probabilityMap[i, j] += n;
                }
            }
        }

        private int WaysShipCanUseCell(int shipSize, int x, int y)
        {
            int n = 0;
            for (int c = x - shipSize; c <= x; c++)
                if (CanShipFit(shipSize, c, y, true))
                    n++;
            for (int c = y - shipSize; c <= y; c++)
                if (CanShipFit(shipSize, c, y, false))
                    n++;
            return n;
        }

        private bool CanShipFit(int shipSize, int x_start, int y_start, bool isHoriziontal)
        {
            if (isHoriziontal)
            {
                for (var i = x_start; i < shipSize; i++)
                {
                    if (i < 0 || i >= 10)
                        return false;
                    if (IsCellOccupied(i, y_start))
                        return false;
                    if (HasAdjacents(i, y_start))
                        return false;
                }
                return true;
            }
            else
            {
                for (var i = y_start; i < shipSize; i++)
                {
                    if (i < 0 || i >= 10)
                        return false;
                    if (IsCellOccupied(x_start, i))
                        return false;
                    if (HasAdjacents(x_start, i))
                        return false;
                }
                return true;
            }
        }

        private bool HasAdjacents(int x, int y)
        {
            for (var i=x-1; i<=x+1; i++)
                for (var j = y - 1; j <= y + 1; j++)
                {
                    if (i < 0 || i >= 10 || j < 0 || j >= 10)
                        continue;
                    if (IsCellOccupied(i, j))
                        return false;
                }
            return true;
        }

        private bool IsCellOccupied(int i, int j)
        {
            return boardState[i, j] == CellStates.Sunk || boardState[i, j] == CellStates.Hit;
        }

        private int[] shipsToSink = new int[] {4, 3, 2, 1};
        private CellStates[,] boardState = new CellStates[10, 10];
        private int[,] probabilityMap = new int[10, 10];
    }

    enum CellStates
    {
        Unknown,
        Water,
        Hit,
        Sunk
    }

    class BoardState
    {
    }
}
