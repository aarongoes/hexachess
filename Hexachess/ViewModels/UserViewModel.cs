using System.Collections.Generic;
using Models;

namespace Hexachess.Models
{
    public class UserViewModel : IGame
    {
        public string Name;
        public List<GameViewModel> Games;
        public int GamesPlayed;
        public int GamesWon;

        public UserViewModel(User user)
        {
            Name = user.Name;
            Games = new List<GameViewModel>();
            if (user.Games != null)
            {
                foreach (Game game in user.Games)
                {
                    Games.Add(new GameViewModel(game));
                    if (game.Winner == 1 && game.FirstPlayer.Token == user.Token)
                    {
                        GamesWon++;
                    }
                }
                GamesPlayed = Games.Count;
                Games.Reverse();
            }
        }
    }
}