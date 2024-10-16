﻿using System.Collections.Generic;
using System.Linq;

namespace Models.ChessPieceObjects
{
    public class Queen : ChessPiece
    {

        public Queen(int player): base(player, Enums.ChessPieces.Queen, 90)
        {
        }

        public override void SetPossibleMoves(Hexagon position, List<Hexagon> hexes)
        {
            PossibleMoves.Clear();
            setKingMoves(position, hexes);
            setRookMoves(position, hexes);
            setBishopMoves(position, hexes);
        }

        private void setKingMoves(Hexagon position, List<Hexagon> hexes)
        {
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
        private void setRookMoves(Hexagon position, List<Hexagon> hexes)
        {
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

        private void setBishopMoves(Hexagon position, List<Hexagon> hexes)
        {
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
