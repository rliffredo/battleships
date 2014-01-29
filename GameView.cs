using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battleships
{
    class GameView : Battleships.IGameView
    {
        public void SetGameInfo(int size, int gameId)
        {
            Console.WriteLine("Game {0} on a {1}x{1} board.", gameId, size+1);
        }
        public void AddShips(IEnumerable<ShipInfo> ships) {
            foreach (var ship in ships)
            {
                Console.Write("Ship ({0}): ", ship.Size);
                foreach (var cell in ship.PositionCells)
                {
                    Console.Write("[{0}, {1}] ", cell.x, cell.y);
                }
                Console.WriteLine("");
            }
        }
        public void AddShot(CellCoords cell, ShotResult result)
        {
            //Console.Write("[{0}, {1}]:{2} ", cell.x, cell.y, GetShotResult(result));
            Console.Write(GetShotResult(result));
        }
        public void SetResults(int shoots, int points, int bestResult)
        {
            Console.WriteLine("\n-> shoots: {0}; current score: {1}; best result: {2}", shoots, points, bestResult);
        }

        private static string GetShotResult(ShotResult result)
        {
            switch (result)
            {
                case ShotResult.Miss:
                    return ".";
                case ShotResult.Hit:
                    return "x";
                case ShotResult.HitAndSunk:
                    return "X";
            }

            return " ";
        }

    }
}
