using System.Collections.Generic;
using Models;

namespace Hexachess.Models
{
    public class PlayersInfoViewModel
    {
        public List<UserInfo> Players;

        public PlayersInfoViewModel()
        {
            Players = new List<UserInfo>();
        }
    }
}