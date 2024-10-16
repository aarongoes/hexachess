using System;
using System.Collections.Generic;

namespace Models.ChessPieceObjects
{
    public class Empty : ChessPiece
    {
        public Empty() : base(0,Enums.ChessPieces.None)
        {
            
        }

        public override void SetPossibleMoves(Hexagon position, List<Hexagon> hexes)
        {
            throw new NotImplementedException();
        }
    }
}
