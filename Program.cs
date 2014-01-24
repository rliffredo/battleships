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
                var gameId = gameApi.CreateNewGame();
                var result = gameApi.Shoot(gameId, 5, 5);
                Console.WriteLine("current score: " + gameApi.GetCurrentScore());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
