using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Battleships
{
    class Decision
    {
        Tuple<int, int> CellToAttack()
        {
            var r = new Random();
            return Tuple.Create(r.Next(10), r.Next(10));
        }
    }

    enum CellStates
    {
        Water,
        Hit,
        Sunk,
        Nil
    }
    class BoardState
    {
    }
}
