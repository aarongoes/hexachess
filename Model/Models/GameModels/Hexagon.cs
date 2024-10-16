using Models.Enums;
using Models.ChessPieceObjects;

namespace Models
{
    public class Hexagon
    {
        public decimal X { get; private set; }
        public decimal Y { get; private set; }
        public HexagonColors Color { get; set; }
        public ChessPiece Piece { get; set; }
        
        public Hexagon(decimal x, decimal y, HexagonColors color = HexagonColors.Empty)
        {
            Color = color;
            X = x;
            Y = y;
            Piece = new Empty();
        }
    }
}
