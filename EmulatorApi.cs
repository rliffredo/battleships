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
        private IGameView _view;
        private IList<ShipInfo> _ships;
        private IList<CellCoords> _shots;
        private IList<int> _scores = new List<int>();
        private int _bestScore;

        public EmulatorApi(IGameView view)
        {
            _view = view;
        }

        public string CreateNewGame()
        {
            _ships = ShipInfo.CreateGameShips();
            _shots = new List<CellCoords>();
            foreach (var ship in _ships.OrderByDescending(s => s.Size))
            {
                var cells = GetCellsForShip(ship.Size, 10);
                ship.PositionCells.Clear();
                foreach (var cell in cells)
                {
                    ship.PositionCells.Add(cell);
                }
            }
            _view.AddShips(_ships);
            return "42";
        }

        private IEnumerable<CellCoords> GetCellsForShip(int shipSize, int boardSize)
        {
            // Not really the most optimized approach, but should be good enough...
            while (true)
            {
                var shipCells = CreateRandomShip(shipSize, boardSize);
                var isShipLegal = 
                    shipCells
                        .SelectMany(c => c.GetSurroundingCells())
                        .Union(shipCells)
                        .Intersect(_ships.SelectMany(s => s.PositionCells))
                        .Union(shipCells
                            .Where(c => c.x < 0 || c.y < 0 || c.x >= boardSize || c.y >= boardSize))
                        .ToList()
                        .Count == 0;
                if (isShipLegal)
                    return shipCells;
            }
        }

        private static List<CellCoords> CreateRandomShip(int shipSize, int boardSize)
        {
            var r = new Random();
            var shipCells = new List<CellCoords>();
            var direction = r.Next(1) == 0;
            var initialPos = new CellCoords(r.Next(boardSize), r.Next(boardSize));
            for (var i = 0; i < shipSize; ++i)
            {
                shipCells.Add(direction ? initialPos.AddHorizontal(i) : initialPos.AddVertical(i));
            }
            return shipCells;
        }

        public int GetCurrentScore()
        {
            return _bestScore;
        }

        public GameState Shoot(string gameId, int row, int column)
        {
            if (_shots.Count > 100)
                throw new Exception("Too many shots!");

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
            var state = new GameState {
                IsFinished = _ships.Count(s => s.IsSunken == false) == 0,
                LastShot = hitShip.IsSunken ? ShotResult.HitAndSunk : ShotResult.Hit
            };

            if (state.IsFinished)
            {
                CalculateScore(_shots.Count);
            }

            return state;
        }

        private void CalculateScore(int shoots)
        {
            _scores.Add(shoots);
            var currentScore = _scores
                 .Skip(_scores.Count - 10)
                 .Select(score => 100 - score)
                 .Sum();
            if (currentScore > _bestScore)
                _bestScore = currentScore;
        }

        public int GetBestScore()
        {
            return _scores.Max();
        }
    }
}
