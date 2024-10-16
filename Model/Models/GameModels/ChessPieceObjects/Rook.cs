using System.Collections.Generic;
using System.Linq;

namespace Models.ChessPieceObjects
{
    public class Rook : ChessPiece
    {

        public Rook(int player): base(player, Enums.ChessPieces.Rook, 50)
        {
        }
        public override void SetPossibleMoves(Hexagon position, List<Hexagon> hexes)
        {
            PossibleMoves.Clear();
            for(int i = 0; i < 6; i++)
            {
                var neighBours = getNeighBours(position, hexes);
                for (int j = 0; j < 11; j++)
                {
                    if (neighBours[i] != null)
                    {
                        var newPosition = hexes.FirstOrDefault(h => h.X == neighBours[i].X && h.Y == neighBours[i].Y);
                        if (newPosition != null)
                        {
                            if (newPosition.Piece.Player == Player)
                            {
                                break;
                            }

                            if (newPosition.Piece.Player > 0)
                            {
                                PossibleMoves.Add(new Move(position, neighBours[i]));
                                break;
                            }
                            
                            PossibleMoves.Add(new Move(position, neighBours[i]));
                        }

                        neighBours = getNeighBours(neighBours[i], hexes);
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        private List<Hexagon> getNeighBours(Hexagon hex, List<Hexagon> hexes)
        {
            List<Hexagon> neighbours = new List<Hexagon>();
            var pairs = new List<KeyValuePair<decimal, decimal>>
            {
                new KeyValuePair<decimal, decimal>(0, 1),
                new KeyValuePair<decimal, decimal>(1, (decimal)0.5),
                new KeyValuePair<decimal, decimal>(1, (decimal)-0.5),
                new KeyValuePair<decimal, decimal>(0, -1),
                new KeyValuePair<decimal, decimal>(-1, (decimal)-0.5),
                new KeyValuePair<decimal, decimal>(-1, (decimal)0.5)
            };

            foreach (var (key, value) in pairs)
            {
                var searchXPos = hex.X + key;
                var searchYPos = hex.Y + value;
                var neighbour = hexes.FirstOrDefault(h => h.X == searchXPos && h.Y == searchYPos);
                neighbours.Add(neighbour);
            }

            return neighbours;
        }
    }
}
