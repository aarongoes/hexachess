using System;
using System.Collections.Generic;
using Models.Utils;

namespace Models
{
    public class Game : DatabaseItem
    {
        [Property] public Guid Token { get; private set; }
        [Property] public int Mode { get; private set; }
        [Property] public int Winner { get; private set; }
        [Property] public DateTime DateStart { get; private set; }
        
        [Property] public DateTime DateEnd { get; private set; }
        [Property] public byte[] Thumbnail { get; set; }
        [Property] public int FirstPlayerId { get; set; }
        [Property] public int? SecondPlayerId { get; set; }
        public List<Move> Moves { get; set; }
        public User FirstPlayer { get; set; }
        public User SecondPlayer { get; set; }
        
        public Game()
        {
        }
        
        public Game(Guid token, int mode, int firstPlayerId)
        {
            Token = token;
            Mode = mode;
            Moves = new List<Move>();
            FirstPlayerId = firstPlayerId;
            Thumbnail = Convert.FromBase64String("Ci==");
        }
    }
}