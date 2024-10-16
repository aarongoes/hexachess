using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Html;
using Models;

namespace Hexachess.Models
{
    public class GameViewModel : IGame
    {
        public Guid Token;
        public List<MoveViewModel> Moves;
        public int MoveCount;
        public int Mode;
        public DateTime DateStart;
        public DateTime DateEnd;
        public HtmlString Thumbnail;
        public UserViewModel FirstPlayer;
        public UserViewModel SecondPlayer;

        public GameViewModel(Game game)
        {
            Token = game.Token;
            Moves = new List<MoveViewModel>();
            foreach (var move in game.Moves)
            {
                Moves.Add(new MoveViewModel(move));
            }
            MoveCount = game.Moves.Count;
            Mode = game.Mode;
            DateStart = game.DateStart;
            DateEnd = game.DateEnd;
            Thumbnail = new HtmlString(Encoding.UTF8.GetString(game.Thumbnail));
            if (game.FirstPlayer != null)
            {
                FirstPlayer = new UserViewModel(game.FirstPlayer);
            }
            if (game.SecondPlayer != null)
            {
                SecondPlayer = new UserViewModel(game.SecondPlayer);
            }
        }
    }
}