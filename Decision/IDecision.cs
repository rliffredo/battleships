using System;
namespace Battleships.Decision
{
    interface IDecision
    {
        CellCoords CellToAttack();
        void UpdateWithFeedback(int x, int y, ShotResult result);
    }
}
