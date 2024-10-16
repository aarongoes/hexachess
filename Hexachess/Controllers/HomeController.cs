using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Hexachess.Models;
using Logic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Models;
using Models.Enums;

namespace Hexachess.Controllers
{
    [Authorize, ExcludeFromCodeCoverage]
    public class HomeController : Controller
    {
        private readonly IUserLogic userLogic;
        private readonly IGameLogic gameLogic;
        private readonly IMoveLogic moveLogic;
        
        public HomeController(Factory.Factory factory)
        {
            gameLogic = factory.GetGameLogic(Engine.MySql);
            userLogic = factory.GetUserLogic(Engine.MySql);
            moveLogic = factory.GetMoveLogic(Engine.MySql);
        }
        public IActionResult Index()
        {
            var user = userLogic.Read(new KeyValuePair<string, object>("Token", User.FindFirst("Token").Value));
            var games = gameLogic.ReadAll(new Dictionary<string, object>{{"FirstPlayerId", user.Id},{"SecondPlayerId", user.Id}});
            
            for(int i = 0; i < games.Count; i++)
            {
                var moves = moveLogic.ReadAll(new Dictionary<string, object>{{"GameId", games[i].Id}});
                games[i] = gameLogic.AssignMoves(games[i], moves);
                var player1 = userLogic.Read(new KeyValuePair<string, object>("Id", games[i].FirstPlayerId));
                User player2 = null;
                if (games[i].SecondPlayerId != null)
                {
                    player2 = userLogic.Read(new KeyValuePair<string, object>("Id", games[i].SecondPlayerId));
                }
                games[i] = gameLogic.AssignPlayers(games[i], player1, player2);
            }
            user = userLogic.AssignGames(user, games);
            
            return View(new UserViewModel(user));
        }
        [HttpGet("/games/{username}")]
        public IActionResult Index(string username)
        {
            var user = userLogic.Read(new KeyValuePair<string, object>("Name", username));
            var games = gameLogic.ReadAll(new Dictionary<string, object>{{"FirstPlayerId", user.Id},{"SecondPlayerId", user.Id}});
            
            for(int i = 0; i < games.Count; i++)
            {
                var moves = moveLogic.ReadAll(new Dictionary<string, object>{{"GameId", games[i].Id}});
                games[i] = gameLogic.AssignMoves(games[i], moves);
                var player1 = userLogic.Read(new KeyValuePair<string, object>("Id", games[i].FirstPlayerId));
                User player2 = null;
                if (games[i].SecondPlayerId != null)
                {
                    player2 = userLogic.Read(new KeyValuePair<string, object>("Id", games[i].SecondPlayerId));
                }
                games[i] = gameLogic.AssignPlayers(games[i], player1, player2);
            }
            user = userLogic.AssignGames(user, games);
            
            return View(new UserViewModel(user));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
        [HttpGet("/games/{username}/home/dutch")]
        [Route("/home/dutch")]
        public IActionResult Dutch(string username = null)
        {
            //Set cookie for dutch language
            var value = CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(new CultureInfo("nl-NL"), new CultureInfo("nl-NL")));
            var options = new CookieOptions { Expires = DateTime.Now.AddDays(1)};
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, value, options);
            
            //Redirect back to index with new language
            return RedirectToAction("Index", "Home", new { username });
        }
        
        [HttpGet("/games/{username}/home/english")]
        [Route("/home/english")]
        public IActionResult English(string username = null)
        {
            //Set cookie for english language
            var value = CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(new CultureInfo("en-US"), new CultureInfo("en-US")));
            var options = new CookieOptions { Expires = DateTime.Now.AddDays(1)};
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, value, options);
            
            //Redirect back to index with new language
            return RedirectToAction("Index", "Home", new { username });
        }
    }
}
