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


            //var d = new Decision();
            //var game = new RestApi();
            //var gameId = game.CreateNewGame();
            //GameState state;
            //var shoots = 0;
            //do
            //{
            //    shoots++;
            //    var coords = d.CellToAttack();
            //    state = game.Shoot(gameId, coords.Item1, coords.Item2);
            //} while (!state.IsFinished);
            //Console.WriteLine("Total shoots: " + shoots);

            try
            {
                var gameApi = new RestApi();
                for (var i = 0; i < 10; ++i)
                {
                    Console.WriteLine("Game {0}:", i + 1);
                    PlayGame(gameApi);
                }
                Console.WriteLine("current score: " + gameApi.GetCurrentScore());
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

        private static void PlayGame(RestApi gameApi)
        {
            var gameId = gameApi.CreateNewGame();
            var shotCount = 1;
            for (var row = 0; row < 10; ++row)
            {
                for (var column = 0; column < 10; ++column)
                {
                    var result = gameApi.Shoot(gameId, row, column);
                    Console.Write(GetShotResult(result.LastShot));
                    if (result.IsFinished)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Done in {0} shots.", shotCount);
                        return;
                    }
                    ++shotCount;
                }
                Console.WriteLine();
            }
        }
    }
}
