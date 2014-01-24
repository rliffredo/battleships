using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;

namespace Battleships
{
    class Program
    {
        static void Main(string[] args)
        {
            var d = new Decision();
            var game = new RestApi();
            var gameId = game.CreateNewGame();
            GameState state;
            do
            {
                var coords = d.CellToAttack();
                state = game.Shoot(gameId, coords.Item1, coords.Item2);
            } while (!state.IsFinished);
        }
    }
}
