namespace Battleships
{
    internal class RestApi
    {
        private const string AuthHeader = "X-Auth-ShipsApiKey";
        private const string UserToken = "3907c0dfa1ce4c59be420815085d9c4b";

        public string CreateNewGame()
        {
            // TODO:
            // send POST to /Game
            return string.Empty;
        }

        public GameState Shoot(int x, int y)
        {
            // TODO:
            // send POST to /Attack/{GameId}
            // convert (x, y) to B5, etc.
            // convert response to shotresult
            return new GameState
            {
                IsFinished = false,
                LastShot = ShotResult.Miss
            };
        }

        public int GetCurrentScore()
        {
            // TODO
            // GET /Score
            return 0;
        }
    }
}