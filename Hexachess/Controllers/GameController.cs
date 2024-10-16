using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Globalization;
using System.Threading.Tasks;
using Hexachess.Hubs;
using Hexachess.Models;
using Logic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.SignalR;
using Models;
using Models.Enums;

namespace Hexachess.Controllers
{
    [Authorize, ExcludeFromCodeCoverage]
    public class GameController : Controller
    {
        private readonly IGameLogic gameLogic;
        private readonly IMoveLogic moveLogic;
        private readonly IBoardLogic boardLogic;
        private readonly IUserLogic userLogic;
        private readonly IApplicationLogic applicationLogic;
        private readonly JsonSerializerSettings jsonSerializerSettings;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IHubContext<MoveHub> hubContext;

        public GameController(Factory.Factory factory, IHttpContextAccessor contextAccessor, IHubContext<MoveHub> hubContext)
        {
            this.hubContext = hubContext;
            gameLogic = factory.GetGameLogic(Engine.MySql);
            moveLogic = factory.GetMoveLogic(Engine.MySql);
            userLogic = factory.GetUserLogic(Engine.MySql);
            boardLogic = factory.GetBoardLogic(factory.GetHexagonLogic());
            applicationLogic = factory.GetApplicationLogic(gameLogic, moveLogic, boardLogic, userLogic);
            _contextAccessor = contextAccessor;
            jsonSerializerSettings = new JsonSerializerSettings
            { Formatting = Formatting.Indented, ContractResolver = new CamelCasePropertyNamesContractResolver() };
        }
        
        
        [HttpGet("/game/{token}")]
        public IActionResult Index(string token = null)
        {
            Game game = applicationLogic.LoadGame(token ?? HttpContext.Request.Cookies["CurrentGameToken"], User.FindFirst("Token").Value);
            if (game == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var options = new CookieOptions();
            options.Expires = DateTimeOffset.Now.AddMinutes(1);
            HttpContext.Response.Cookies.Append("CurrentGameToken", game.Token.ToString(), options);
            return View(new GameViewModel(game));
        }
        
        [HttpGet("/game/{token}/join")]
        public async Task<IActionResult> Join(string token = null)
        {
            applicationLogic.JoinGame(token, User.FindFirst("Token").Value);
            await hubContext.Clients.Group(token).SendAsync("PlayerJoined");
            return RedirectToAction("Index", "Game", new { token });
        }
        [HttpGet("/game/new/{gamemode}")]
        public IActionResult New(int gamemode = 0)
        {
            Game game = applicationLogic.CreateGame(User.FindFirst("Token").Value, gamemode);
            var options = new CookieOptions();
            options.Expires = DateTimeOffset.Now.AddMinutes(1);
            HttpContext.Response.Cookies.Append("CurrentGameToken", game.Token.ToString(), options);
            return RedirectToAction("Index", "Game", new { token = game.Token.ToString() });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost("/movepiece")]
        public async void MovePiece(Move chessMove, string gameToken)
        {
            chessMove.ConvertDecimals();
            Board board = applicationLogic.MovePiece(chessMove, gameToken, User.FindFirst("Token").Value);
            var options = new CookieOptions();
            options.Expires = DateTimeOffset.Now.AddMinutes(1);
            HttpContext.Response.Cookies.Append("CurrentGameToken", board.Game.Token.ToString(), options);
            await hubContext.Clients.Group(gameToken).SendAsync("BoardUpdate");
        }
        
        [HttpPost("/save")]
        public void Save(string value, string token)
        {
            var decoded = Convert.FromBase64String(value);
            var game = gameLogic.Read(new KeyValuePair<string, object>("Token", token));
            game = gameLogic.AssignThumbnail(game, decoded);
            gameLogic.Update(new KeyValuePair<string, object>("Token", game.Token), new KeyValuePair<string, object>("Thumbnail", decoded));
        }
        
        [HttpPost("/loadgame")]
        public IActionResult LoadGame(string gameToken)
        {
            var game = gameLogic.Read(new KeyValuePair<string, object>("Token", gameToken));
            var moves = moveLogic.ReadAll(new Dictionary<string, object>{{"GameId", game.Id}});
            game = gameLogic.AssignMoves(game, moves);
            var returnBoard = boardLogic.LoadBoard(game);
            var nextPlayer = boardLogic.GetNextPlayer(returnBoard);
            var options = new CookieOptions();
            options.Expires = DateTimeOffset.Now.AddMinutes(1);
            HttpContext.Response.Cookies.Append("CurrentGameToken", returnBoard.Game.Token.ToString(), options);
            return Json(new {success = true, returnBoard.Grid, returnBoard.Game.Token, nextPlayer, game.Mode}, jsonSerializerSettings);
        }
        
        [HttpPost("/loadstats")]
        public IActionResult LoadStats(string gameToken = null)
        {
            Game game = null;
            if (gameToken != null)
            {
                game = gameLogic.Read(new KeyValuePair<string, object>("Token", gameToken));
                var moves = moveLogic.ReadAll(new Dictionary<string, object>{{"GameId", game.Id}});
                game = gameLogic.AssignMoves(game, moves);
                var player1 = userLogic.Read(new KeyValuePair<string, object>("Id", game.FirstPlayerId));
                User player2 = null;
                if (game.SecondPlayerId != null)
                {
                    player2 = userLogic.Read(new KeyValuePair<string, object>("Id", game.SecondPlayerId));
                }
                game = gameLogic.AssignPlayers(game, player1, player2);
            }
            GameViewModel model = new GameViewModel(game);
            return PartialView("Moves", model);
        }
        
        [HttpPost("/loadplayersinfo")]
        public IActionResult LoadPlayersInfo(string gameToken = null)
        {
            Game game;
            PlayersInfoViewModel model = new PlayersInfoViewModel();
            if (gameToken != null)
            {
                game = gameLogic.Read(new KeyValuePair<string, object>("Token", gameToken));
                model.Players.Add(userLogic.GetUserInfo(game.FirstPlayerId));
                if (game.SecondPlayerId != null) model.Players.Add(userLogic.GetUserInfo((int)game.SecondPlayerId));
            }
            return PartialView("Players", model);
        }
        
        public IActionResult Dutch()
        {
            var value = CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(new CultureInfo("nl-NL"), new CultureInfo("nl-NL")));
            var options = new CookieOptions { Expires = DateTime.Now.AddDays(1)};
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, value, options);
            return RedirectToAction("Index", "Game", new {token = HttpContext.Request.Cookies["CurrentGameToken"]});
        }
        public IActionResult English()
        {
            var value = CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(new CultureInfo("en-US"), new CultureInfo("en-US")));
            var options = new CookieOptions { Expires = DateTime.Now.AddDays(1)};
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, value, options);
            return RedirectToAction("Index", "Game", new {token = HttpContext.Request.Cookies["CurrentGameToken"]});
        }
    }
}