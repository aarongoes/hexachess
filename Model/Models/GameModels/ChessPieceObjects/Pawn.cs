using System.Collections.Generic;

namespace Models.ChessPieceObjects
{
    public class Pawn : ChessPiece
    {
        public Pawn(int player): base(player, Enums.ChessPieces.Pawn, 10)
        {
        }

        public override void SetPossibleMoves(Hexagon position, List<Hexagon> hexes)
        {
            PossibleMoves.Clear();
            Hexagon newPosition;
            if (Player == 1)
            {
                newPosition = hexes.Find(h => h.X == position.X && h.Y == position.Y + 1);
                if (newPosition != null)
                {
                    if (newPosition.Piece.Player == 0)
                    {
                        PossibleMoves.Add(new Move(position, newPosition));
                        
                        if (hasMoved(position))
                        {
                            newPosition = hexes.Find(h => h.X == position.X && h.Y == position.Y + 2);
                            if (newPosition != null)
                            {
                                if (newPosition.Piece.Player == 0)
                                {
                                    PossibleMoves.Add(new Move(position, newPosition));
                                }
                            }
                        }
                    }
                }

                newPosition = hexes.Find(h => h.X == position.X - 1 && h.Y == position.Y + (decimal) 0.5);
                if (newPosition != null)
                {
                    if (newPosition.Piece.Player != position.Piece.Player && newPosition.Piece.Player > 0)
                    {
                        PossibleMoves.Add(new Move(position, newPosition));
                    }
                }
                newPosition = hexes.Find(h => h.X == position.X + 1 && h.Y == position.Y + (decimal) 0.5);
                if (newPosition != null)
                {
                    if (newPosition.Piece.Player != position.Piece.Player && newPosition.Piece.Player > 0)
                    {
                        PossibleMoves.Add(new Move(position, newPosition));
                    }
                }
            }
            else
            {
                newPosition = hexes.Find(h => h.X == position.X && h.Y == position.Y - 1);
                if (newPosition != null)
                {
                    if (newPosition.Piece.Player == 0)
                    {
                        PossibleMoves.Add(new Move(position, newPosition));
                        
                        if (hasMoved(position))
                        {
                            newPosition = hexes.Find(h => h.X == position.X && h.Y == position.Y - 2);
                            if (newPosition != null)
                            {
                                if (newPosition.Piece.Player == 0)
                                {
                                    PossibleMoves.Add(new Move(position, newPosition));
                                }
                            }
                        }
                    }
                }

                newPosition = hexes.Find(h => h.X == position.X - 1 && h.Y == position.Y - (decimal) 0.5);
                if (newPosition != null)
                {
                    if (newPosition.Piece.Player != position.Piece.Player && newPosition.Piece.Player > 0)
                    {
                        PossibleMoves.Add(new Move(position, newPosition));
                    }
                }
                newPosition = hexes.Find(h => h.X == position.X + 1 && h.Y == position.Y - (decimal) 0.5);
                if (newPosition != null)
                {
                    if (newPosition.Piece.Player != position.Piece.Player && newPosition.Piece.Player > 0)
                    {
                        PossibleMoves.Add(new Move(position, newPosition));
                    }
                }
            }
        }
        public bool hasMoved(Hexagon position)
        {
            var requiredAmount = 9;
            var center = 0;
            var left = 0;
            var right = 0;
            decimal yAxis = - 1;
            var amountOfPiecesPlacedPerTime = 2;
            
            if (Player == 1)
            {
                if (position.X == center && position.Y == yAxis)
                {
                    return true;
                }
                requiredAmount--;
                for (int i = 0; i <= requiredAmount; i += amountOfPiecesPlacedPerTime)
                {
                    if (position.X == left && position.Y == yAxis)
                    {
                        return true;
                    }
                    if (position.X == right && position.Y == yAxis)
                    {
                        return true;
                    }
                    left--;
                    right++;
                    yAxis -= (decimal)0.5;
                }
                return false;
            }

            yAxis = 1;
            left = 0;
            right = 0;
            if (position.X == center && position.Y == yAxis)
            {
                return true;
            }
            requiredAmount--;
            for (int i = 0; i <= requiredAmount; i += amountOfPiecesPlacedPerTime)
            {
                if (position.X == left && position.Y == yAxis)
                {
                    return true;
                }
                if (position.X == right && position.Y == yAxis)
                {
                    return true;
                }
                left--;
                right++;
                yAxis += (decimal)0.5;
            }
            return false;
        }
        public bool HasReachedEnd(Hexagon position)
        {
            if (Player == 2)
            {
                if (position.Y == 0)
                {
                    return false;
                }
                return true;
            }
            if (Player == 1)
            {
                var x = 0;
                var y = 5;
                bool switcher = false;
                for (int i = 0; i < 11; i++)
                {
                    if (position.X == x && position.Y == y)
                    {
                        return false;
                    }
                    if (y == 5)
                    {
                        switcher = true;
                    }
                    if (switcher)
                    {
                        y--;
                    }
                    else
                    {
                        y++;
                    }
                    x++;
                }
                return true;
            }
            return false;
        }
    }
}
