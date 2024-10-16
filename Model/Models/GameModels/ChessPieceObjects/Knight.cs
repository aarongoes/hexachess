using System.Collections.Generic;
using System.Linq;

namespace Models.ChessPieceObjects
{
    public class Knight : ChessPiece
    {

        public Knight(int player): base(player, Enums.ChessPieces.Knight, 30)
        {
        }

        public override void SetPossibleMoves(Hexagon position, List<Hexagon> hexes)
        {
            PossibleMoves.Clear();
            var pairs = new List<KeyValuePair<decimal, decimal>>
            {
                new KeyValuePair<decimal, decimal>(1, (decimal)2.5),
                new KeyValuePair<decimal, decimal>(2, 2),
                new KeyValuePair<decimal, decimal>(3, (decimal)0.5),
                new KeyValuePair<decimal, decimal>(3, (decimal)-0.5),
                new KeyValuePair<decimal, decimal>(2, -2),
                new KeyValuePair<decimal, decimal>(1, (decimal)-2.5),
                new KeyValuePair<decimal, decimal>(-1, (decimal)-2.5),
                new KeyValuePair<decimal, decimal>(-2, -2),
                new KeyValuePair<decimal, decimal>(-3, (decimal)-0.5),
                new KeyValuePair<decimal, decimal>(-3, (decimal)0.5),
                new KeyValuePair<decimal, decimal>(-2, 2),
                new KeyValuePair<decimal, decimal>(-1, (decimal)2.5)
            };
            foreach (var (key, value) in pairs)
            {
                var searchXPos = position.X + key;
                var searchYPos = position.Y + value;
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
