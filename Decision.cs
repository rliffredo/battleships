using System;
using System.Collections;
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
            ChaseHits();
            RemoveKnownCells();
            //PrintProbabilityMap();
            return PickBestCell();
        }

        private void PrintProbabilityMap()
        {
            for (var i = 0; i < 10; i++)
            {
                for (var j = 0; j < 10; j++)
                {
                    Console.Write(probabilityMap[j,i] + "\t");
                }
                Console.Write("\n");
            }
            Console.ReadKey();
        }

        private void ChaseHits()
        {
            for (var i = 0; i < 10; i++)
            {
                for (var j = 0; j < 10; j++)
                {
                    if (boardState[i, j] == CellStates.Hit)
                    {
                        IncreaseProbability(i - 1, j);
                        IncreaseProbability(i + 1, j);
                        IncreaseProbability(i, j - 1);
                        IncreaseProbability(i, j + 1);
                    }
                }
            }
        }

        private void IncreaseProbability(int x, int y)
        {
            if (x >= 0 && y >= 0 && x < 10 && y < 10)
                probabilityMap[x, y] += 10;
        }

        private void RemoveKnownCells()
        {
            for (var i = 0; i < 10; i++)
            {
                for (var j = 0; j < 10; j++)
                {
                    if (!IsCellKnown(i, j))
                        probabilityMap[i, j] = -1;
                }
            }
        }

        public void UpdateWithFeedback(int x, int y, ShotResult result)
        {
            if (result == ShotResult.Hit)
                MarkCurrentShipAsHit(x, y);
            if (result == ShotResult.HitAndSunk)
                MarkCurrentShipAsSunk(x, y);
            if (result == ShotResult.Miss)
                boardState[x, y] = CellStates.Water;
        }

        private void MarkCurrentShipAsSunk(int x, int y)
        {
            var shipCells = GetSunkenShip(x, y);
            foreach (var cell in shipCells)
            {
                boardState[cell.Item1, cell.Item2] = CellStates.Sunk;
            }
            shipsToSink[shipCells.Count-1]--;
        }

        private IList<Tuple<int, int>> GetSunkenShip(int x, int y)
        {
            var ret = new List<Tuple<int, int>>();
            ret.Add(Tuple.Create(x, y));
            for (var xpos = x-1; xpos>=0; xpos--)
                if (boardState[xpos, y] == CellStates.Hit)
                    ret.Add(Tuple.Create(xpos, y));
                else
                    break;
            for (var xpos = x+1; xpos < 10; xpos++)
                if (boardState[xpos, y] == CellStates.Hit)
                    ret.Add(Tuple.Create(xpos, y));
                else
                    break;

            for (var ypos = y-1; ypos >= 0; ypos--)
                if (boardState[x, ypos] == CellStates.Hit)
                    ret.Add(Tuple.Create(x, ypos));
                else
                    break;
            for (var ypos = y+1; ypos < 10; ypos++)
                if (boardState[x, ypos] == CellStates.Hit)
                    ret.Add(Tuple.Create(x, ypos));
                else
                    break;
            return ret;
        }

        private void MarkCurrentShipAsHit(int x, int y)
        {
            boardState[x, y] = CellStates.Hit;
            lastHit = Tuple.Create(x, y);
        }

        private Tuple<int, int> PickBestCell()
        {
            var ret = Tuple.Create(0, 0);
            for (var i = 0; i < 10; i++)
            {
                for (var j = 0; j < 10; j++)
                {
                    if (probabilityMap[i, j] > probabilityMap[ret.Item1, ret.Item2])
                        ret = Tuple.Create(i, j);
                }
            }
            return ret;
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
                    probabilityMap[i, j] += n * shipsToSink[shipType];
                }
            }
        }

        private int WaysShipCanUseCell(int shipSize, int x, int y)
        {
            int n = 0;
            for (int pos_x = x - shipSize; pos_x <= x; pos_x++)
                if (CanShipFit(shipSize, pos_x, y, true))
                    n++;
            for (int pos_y = y - shipSize; pos_y <= y; pos_y++)
                if (CanShipFit(shipSize, x, pos_y, false))
                    n++;
            return n;
        }

        private bool CanShipFit(int shipSize, int x_start, int y_start, bool isHorizontal)
        {
            if (x_start < 0 || x_start >= 10)
                return false;
            if (y_start < 0 || y_start >= 10)
                return false;
            if (isHorizontal)
            {
                for (var x = x_start; x <= x_start + shipSize; x++)
                {
                    if (x < 0 || x >= 10)
                        return false;
                    if (!IsCellCandidate(x, y_start))
                        return false;
                    if (HasAdjacents(x, y_start))
                        return false;
                }
                return true;
            }
            else
            {
                for (var y = y_start; y <= y_start + shipSize; y++)
                {
                    if (y < 0 || y >= 10)
                        return false;
                    if (!IsCellCandidate(x_start, y))
                        return false;
                    if (HasAdjacents(x_start, y))
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
                    if (i!=x && j!=y && IsCellOccupied(i, j))
                        return true;
                }
            return false;
        }

        private bool IsCellOccupied(int i, int j)
        {
            return boardState[i, j] == CellStates.Sunk || boardState[i, j] == CellStates.Hit;
        }

        private bool IsCellCandidate(int i, int j)
        {
            return boardState[i, j] == CellStates.Unknown || boardState[i, j] == CellStates.Hit;
        }

        private bool IsCellKnown(int i, int j)
        {
            return boardState[i, j] != CellStates.Unknown;
        }

        private int[] shipsToSink = new int[] {4, 3, 2, 1};
        private CellStates[,] boardState = new CellStates[10, 10];
        private int[,] probabilityMap = new int[10, 10];
        private Tuple<int, int> lastHit;
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
