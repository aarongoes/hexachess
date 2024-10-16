using System.Collections.Generic;
using System.Linq;

namespace Models.ChessPieceObjects
{
    public class Bishop : ChessPiece
    {

        public Bishop(int player): base(player,Enums.ChessPieces.Bishop, 30)
        {
        }
        public override void SetPossibleMoves(Hexagon position, List<Hexagon> hexes)
        {
            PossibleMoves.Clear();
            var direction1 = new List<KeyValuePair<decimal, decimal>>
            {
                new KeyValuePair<decimal, decimal>(2, 0),
                new KeyValuePair<decimal, decimal>(4, 0),
                new KeyValuePair<decimal, decimal>(6, 0),
                new KeyValuePair<decimal, decimal>(8, 0),
                new KeyValuePair<decimal, decimal>(10, 0),
            };
            
            var direction2 = new List<KeyValuePair<decimal, decimal>>
            {
                new KeyValuePair<decimal, decimal>(-2, 0),
                new KeyValuePair<decimal, decimal>(-4, 0),
                new KeyValuePair<decimal, decimal>(-6, 0),
                new KeyValuePair<decimal, decimal>(-8, 0),
                new KeyValuePair<decimal, decimal>(-10, 0),
            };
            
            var direction3 = new List<KeyValuePair<decimal, decimal>>
            {
                new KeyValuePair<decimal, decimal>(1, (decimal)1.5),
                new KeyValuePair<decimal, decimal>(2, 3),
                new KeyValuePair<decimal, decimal>(3, (decimal)4.5),
                new KeyValuePair<decimal, decimal>(4, 6),
                new KeyValuePair<decimal, decimal>(5, (decimal)7.5),
            };
            
            var direction4 = new List<KeyValuePair<decimal, decimal>>
            {
                new KeyValuePair<decimal, decimal>(-1, (decimal)1.5),
                new KeyValuePair<decimal, decimal>(-2, 3),
                new KeyValuePair<decimal, decimal>(-3, (decimal)4.5),
                new KeyValuePair<decimal, decimal>(-4, 6),
                new KeyValuePair<decimal, decimal>(-5, (decimal)7.5),
            };
            
            var direction5 = new List<KeyValuePair<decimal, decimal>>
            {
                new KeyValuePair<decimal, decimal>(1, -(decimal)1.5),
                new KeyValuePair<decimal, decimal>(2, -3),
                new KeyValuePair<decimal, decimal>(3, -(decimal)4.5),
                new KeyValuePair<decimal, decimal>(4, -6),
                new KeyValuePair<decimal, decimal>(5, -(decimal)7.5),
            };
            
            var direction6 = new List<KeyValuePair<decimal, decimal>>
            {
                new KeyValuePair<decimal, decimal>(-1, -(decimal)1.5),
                new KeyValuePair<decimal, decimal>(-2, -3),
                new KeyValuePair<decimal, decimal>(-3, -(decimal)4.5),
                new KeyValuePair<decimal, decimal>(-4, -6),
                new KeyValuePair<decimal, decimal>(-5, -(decimal)7.5)
            };

            var directions = new List<List<KeyValuePair<decimal, decimal>>> {direction1,direction2,direction3,direction4,direction5,direction6};
            
            directions.Add(direction1);
            directions.Add(direction2);
            directions.Add(direction3);
            directions.Add(direction4);
            directions.Add(direction5);
            directions.Add(direction6);

            foreach (var direction in directions)
            {
                foreach (var (key, value) in direction)
                {
                    var searchXPos = position.X + key;
                    var searchYPos = position.Y + value;
                    var newPosition = hexes.FirstOrDefault(h => h.X == searchXPos && h.Y == searchYPos);
                    if (newPosition != null)
                    {
                        if (newPosition.Piece.Player == Player)
                        {
                            break;
                        }
                        if (newPosition.Piece.Player > 0)
                        {
                            PossibleMoves.Add(new Move(position, newPosition));
                            break;
                        }
                        PossibleMoves.Add(new Move(position, newPosition));
                    }
                }
            }
        }
    }
}
