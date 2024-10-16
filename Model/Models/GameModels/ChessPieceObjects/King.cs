using System.Collections.Generic;
using System.Linq;

namespace Models.ChessPieceObjects
{
    public class King : ChessPiece
    {

        public King(int player): base(player, Enums.ChessPieces.King, 900)
        {
        }

        public override void SetPossibleMoves(Hexagon position, List<Hexagon> hexes)
        {
            PossibleMoves.Clear();
            var pairs = new List<KeyValuePair<decimal, decimal>>
            {
                new KeyValuePair<decimal, decimal>(0, 1),
                new KeyValuePair<decimal, decimal>(1, (decimal)0.5),
                new KeyValuePair<decimal, decimal>(1, (decimal)-0.5),
                new KeyValuePair<decimal, decimal>(0, -1),
                new KeyValuePair<decimal, decimal>(-1, (decimal)-0.5),
                new KeyValuePair<decimal, decimal>(-1, (decimal)0.5),
                
                new KeyValuePair<decimal, decimal>(1, (decimal)1.5),
                new KeyValuePair<decimal, decimal>(2, 0),
                new KeyValuePair<decimal, decimal>(1, (decimal)-1.5),
                new KeyValuePair<decimal, decimal>(-1, (decimal)-1.5),
                new KeyValuePair<decimal, decimal>(-2, 0),
                new KeyValuePair<decimal, decimal>(-1, (decimal)1.5)
            };

            foreach (var pair in pairs)
            {
                var searchXPos = position.X + pair.Key;
                var searchYPos = position.Y + pair.Value;
                var newPosition = hexes.FirstOrDefault(h => h.X == searchXPos && h.Y == searchYPos);
                if (newPosition != null)
                {
                    if (newPosition.Piece.Player != Player)
                    {
                        PossibleMoves.Add(new Move(position, newPosition));
                    }
                }
            }
        }
    }
}

//if (newPosition.Piece.Player == Player)
//{
//switch (pairs.IndexOf(pair))
//{
//case 0:
//pairs.Remove(pairs.FirstOrDefault(k => k.Key == -1 && k.Value == (decimal) 1.5));
//pairs.Remove(pairs.FirstOrDefault(k => k.Key == 1 && k.Value == (decimal) 1.5));
//break;
//case 1:
//pairs.Remove(pairs.FirstOrDefault(k => k.Key == 1 && k.Value == (decimal) 1.5));
//pairs.Remove(pairs.FirstOrDefault(k => k.Key == 2 && k.Value == (decimal) 0));
//break;
//case 2:
//pairs.Remove(pairs.FirstOrDefault(k => k.Key == 2 && k.Value == (decimal) 0));
//pairs.Remove(pairs.FirstOrDefault(k => k.Key == 1 && k.Value == (decimal) -1.5));
//break;
//case 3:
//pairs.Remove(pairs.FirstOrDefault(k => k.Key == 1 && k.Value == (decimal) -1.5));
//pairs.Remove(pairs.FirstOrDefault(k => k.Key == -1 && k.Value == (decimal) -1.5));
//break;
//case 4:
//pairs.Remove(pairs.FirstOrDefault(k => k.Key == -1 && k.Value == (decimal) -1.5));
//pairs.Remove(pairs.FirstOrDefault(k => k.Key == -2 && k.Value == (decimal) 0));
//break;
//case 5:
//pairs.Remove(pairs.FirstOrDefault(k => k.Key == -2 && k.Value == (decimal) 0));
//pairs.Remove(pairs.FirstOrDefault(k => k.Key == -1 && k.Value == (decimal) 1.5));
//break;
//}
//}