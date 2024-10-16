using Models;

namespace Logic.Interfaces
{
    public interface IHexagonLogic
    {
        Hexagon RemovePiece(Hexagon hexagon);
        Hexagon AssignPiece(Hexagon hexagon, ChessPiece piece);

    }
}