using System;

namespace Battleships
{
    class Program
    {
        static void Main()
        {
            try
            {
                IGameView gameView = new GameView();
                IGameApi gameApi = new EmulatorApi(gameView); // RestApi();
                for (var i = 0; i < 1000; ++i)
                {
                    gameView.SetGameInfo(CellCoords.MAX, i + 1);
                    var shoots = PlayGame(gameApi, gameView);
                    gameView.SetResults(shoots, gameApi.GetCurrentScore(), gameApi.GetBestScore());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static int PlayGame(IGameApi gameApi, IGameView gameView)
        {
            Decision.IDecision d = new Decision.Decision(); // Decision.DecisionOld();
            var gameId = gameApi.CreateNewGame();
            GameState state;
            var shoots = 0;
            do
            {
                shoots++;
                var cell = d.CellToAttack();
                var coords = cell.AsTuple();
                state = gameApi.Shoot(gameId, coords.Item1, coords.Item2);
                d.UpdateWithFeedback(coords.Item1, coords.Item2, state.LastShot);
                gameView.AddShot(cell, state.LastShot);
            } while (!state.IsFinished);
            return shoots;
        }
    }
}
