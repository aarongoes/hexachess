using System.Collections.Generic;

namespace Models
{
    public class Board
    {
        public List<List<Hexagon>> Grid;
        public List<Hexagon> Hexes;
        public bool Succes { get; set; }
        public int Height { get;}
        public int Width { get;}
        public Game Game { get; }

        public Board(Game game)
        {
            Height = 11;
            Width = 11;
            Grid = new List<List<Hexagon>>();
            Hexes = new List<Hexagon>();
            Game = game;
            Succes = false;
        }
    }
}