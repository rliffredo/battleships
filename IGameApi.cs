using System;
namespace Battleships
{
    interface IGameApi
    {
        string CreateNewGame();
        int GetCurrentScore();
        GameState Shoot(string gameId, int row, int column);
    }
}
