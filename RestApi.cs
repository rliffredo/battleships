using System;
using System.Collections.Generic;
using System.Net;
using RestSharp;

namespace Battleships
{
    internal class RestApi
    {
        private const string ServiceAddress = "https://10.12.216.168";
        private const string AuthHeader = "X-Auth-ShipsApiKey";
        private const string UserToken = "3907c0dfa1ce4c59be420815085d9c4b";
        private const string ContentTypeHeader = "Content-Type";
        private const string ContentTypeHeaderValue = "application/json; charset=utf-8";

        private readonly RestClient _client;

        public RestApi()
        {
            _client = new RestClient(ServiceAddress);
        }

        private IRestResponse<Dictionary<string, string>> SendRequest(string resource, Method method)
        {
            return SendRequest(resource, method, null);
        }

        private IRestResponse<Dictionary<string, string>> SendRequest(string resource, Method method, object body)
        {
            var request = new RestRequest(resource, method);
            request.AddHeader(AuthHeader, UserToken);
            request.AddHeader(ContentTypeHeader, ContentTypeHeaderValue);
            if (body != null)
            {
                request.RequestFormat = DataFormat.Json;
                request.AddBody(body);
            }
            return _client.Execute<Dictionary<string, string>>(request);
        }

        public string CreateNewGame()
        {
            var response = SendRequest("/Game", Method.POST);
            return response.Data["Id"];
        }

        private const string RowNames = "ABCDEFGHIJ";

        private static dynamic CreateShot(int row, int column)
        {
            var r = RowNames.Substring(row, 1);
            var c = column + 1;
            return new { Location = r + c };
        }

        static ShotResult GetShotResult(string resultString)
        {
            switch (resultString)
            {
                case "1":
                    return ShotResult.Miss;
                case "2":
                    return ShotResult.Hit;
                case "3":
                    return ShotResult.HitAndSunk;
                default:
                    throw new Exception("unknown shot result string: " + resultString);
            }
        }

        public GameState Shoot(string gameId, int row, int column)
        {
            var response = SendRequest("/Attack/" + gameId, Method.POST, CreateShot(row, column));
            var shotResult = response.Data["Result"];
            var isFinished = response.Data["IsGameFinished"];
            return new GameState
            {
                IsFinished = isFinished == "True",
                LastShot = GetShotResult(shotResult)
            };
        }

        public int GetCurrentScore()
        {
            var response = SendRequest("/Score", Method.GET);
            return int.Parse(response.Data["Score"]);
        }

        public bool SetNick(string nickName)
        {
            var response = SendRequest("/Nick/" + nickName, Method.PUT);
            return response.StatusCode == HttpStatusCode.OK;
        }

        public object GetPlayers()
        {
            // TODO
            //[{"Id":1,"{"Nick":"MegaZordon","Score":10000,"Picture":"http://res.cloudinary.com/demo/image/gravatar/d_retro/unknown_id.jpg"},
            throw new NotImplementedException();
        }

        public object GetGames()
        {
            // /Games/{PlayerID}
            //[{"Title":"123 - 12:34","Board":["A1","B2","C3","C4","C5"],"Moves":["A1","B1","C1","D1","E1","F1","G1","I1","J1","A2"],”Score”:23},{"Title":"23 - 11:12","Board":["A1","B2","C3","C4","C5"],"Moves":["A1","B1","C1","D1","E1","F1","G1","I1","J1","A2"],”Score”:31}]
            throw new NotImplementedException();
        }
    }
}