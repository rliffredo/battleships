using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships
{
    class EmulatorApi: IGameApi
    {
        private IList<ShipInfo> _ships;
        private IList<CellCoords> _shots;

        public string CreateNewGame()
        {
            _ships = ShipInfo.CreateGameShips();
            var r = new Random();
            foreach (var ship in _ships)
            {
                var direction = r.Next(1) == 0;
                var initialPos = GetFirstCell(ship.Size, direction);
                for (var i = 0; i < ship.Size; ++i)
                {
                    var cell = direction ? initialPos.AddHorizontal(i) : initialPos.AddVertical(i);
                    ship.HitCells.Add(cell);
                }
            }
            return "42";
        }

        private CellCoords GetFirstCell(int p, bool direction)
        {
            throw new NotImplementedException();
        }

        public int GetCurrentScore()
        {
            return 1;
        }

        public GameState Shoot(string gameId, int row, int column)
        {
            Debug.Assert(gameId == "42");

            var shot = new CellCoords(row, column);
            Debug.Assert(!_shots.Contains(shot));
            _shots.Add(shot);
            
            var hitShip = _ships
                .Where(s => s.IsSunken == false)
                .FirstOrDefault(s => s.PositionCells.Contains(shot));
            if (hitShip == null)
            {
                return new GameState { IsFinished = false, LastShot = ShotResult.Miss };
            }

            hitShip.HitCells.Add(shot);
            return new GameState {
                IsFinished = _ships.Count(s => s.IsSunken == false) == 0,
                LastShot = hitShip.IsSunken ? ShotResult.HitAndSunk : ShotResult.Hit
            };
        }
    }
}
