using Logic.Interfaces;
using Models;
using Models.ChessPieceObjects;
using Models.Enums;

namespace Logic
{
    public class HexagonLogic : IHexagonLogic
    {
        public Hexagon RemovePiece(Hexagon hexagon)
        {
            hexagon.Piece = new Empty();
            return hexagon;
        }
        
        public Hexagon AssignPiece(Hexagon hexagon, ChessPiece piece)
        {
            hexagon.Piece = piece;
            return hexagon;
        }
    }
}