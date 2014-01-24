﻿using System;
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
            var d = new Decision();
            var gameId = gameApi.CreateNewGame();
            GameState state;
            var shoots = 0;
            do
            {
                shoots++;
                var coords = d.CellToAttack();
                state = gameApi.Shoot(gameId, coords.Item1, coords.Item2);
                d.UpdateWithFeedback(coords.Item1, coords.Item2, state.LastShot);
            } while (!state.IsFinished);
            Console.WriteLine("Total shoots: " + shoots);
        }
    }
}
