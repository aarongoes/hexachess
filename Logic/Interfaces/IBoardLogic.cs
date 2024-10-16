using Models;

namespace Logic.Interfaces
{
    public interface IBoardLogic
    {
        Board ApplyMove(Board board, Move move);
        Board LoadBoard(Game game);
        int GetNextPlayer(Board board);
        int GetPlayerAssignedToPiecePosition(Board board, decimal x, decimal y);
        Hexagon getHexAtPosition(Board board, decimal x, decimal y);
    }
}