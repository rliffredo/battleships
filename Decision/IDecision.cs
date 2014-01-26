using System;
namespace Battleships.Decision
{
    interface IDecision
    {
        Tuple<int, int> CellToAttack();
        void UpdateWithFeedback(int x, int y, ShotResult result);
    }
}
