using System.Collections.Generic;
using Models.Enums;

namespace Models
{
    public abstract class ChessPiece
    {
        public List<Move> PossibleMoves { get; }
        public int Player { get; }
        public ChessPieces PieceType { get; }
        
        public int Score { get; }

        public ChessPiece(int player, ChessPieces piece, int score = 0)
        {
            Player = player;
            PieceType = piece;
            PossibleMoves = new List<Move>();
            Score = score;
        }

        public abstract void SetPossibleMoves(Hexagon position, List<Hexagon> hexes);
    }
}
