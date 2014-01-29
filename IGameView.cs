using System;
namespace Battleships
{
    interface IGameView
    {
        void AddShips(System.Collections.Generic.IEnumerable<ShipInfo> ships);
        void AddShot(CellCoords cell, ShotResult result);
        void SetGameInfo(int size, int gameId);
        void SetResults(int shots, int points, int bestResult);
    }
}
