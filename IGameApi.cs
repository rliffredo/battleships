using System;
namespace Battleships
{
    interface IGameApi
    {
        string CreateNewGame();
        int GetCurrentScore();
        int GetBestScore();
        GameState Shoot(string gameId, int row, int column);
    }
}
