using System;
using System.Net;

namespace Battleships
{
    class Program
    {
        static void Main()
        {
            // ignore HTTPS certificate errors
            ServicePointManager.ServerCertificateValidationCallback =
                (sender, cert, chain, policy) => true;

            try
            {
                var gameApi = new EmulatorApi(); // RestApi();
                var i = 0;
                while (i < 1000)
                {
                    Console.Write("Game {0}... ", i + 1);
                    var shoots = PlayGame(gameApi);
                    Console.WriteLine("{0} shoots", shoots);
                    ++i;
                    if (i % 10 == 0)
                    {
                        Console.WriteLine("Current score: " + gameApi.GetCurrentScore());
                    }
                }
                Console.WriteLine("Best result: " + gameApi.GetBestScore());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        static string GetShotResult(ShotResult result)
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

        private static int PlayGame(IGameApi gameApi)
        {
            Decision.IDecision d = new Decision.Decision();
            //Decision.IDecision d = new Decision.DecisionOld();
            var gameId = gameApi.CreateNewGame();
            GameState state;
            var shoots = 0;
            do
            {
                shoots++;
                //Console.Write("Shooting at: ");
                var coords = d.CellToAttack();
                //Console.Write("[{0}, {1}]: ", coords.Item1, coords.Item2);
                state = gameApi.Shoot(gameId, coords.Item1, coords.Item2);
                // Console.WriteLine(state.LastShot);
                d.UpdateWithFeedback(coords.Item1, coords.Item2, state.LastShot);
                //Console.Write(GetShotResult(state.LastShot));
                //Console.Write("; ");
            } while (!state.IsFinished);
            return shoots;
        }
    }
}
