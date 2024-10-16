using Models;

namespace Logic.Interfaces
{
    public interface IMoveLogic : ILogic<Move>
    {
        Move Create(decimal oldX, decimal oldY, decimal newX, decimal newY, int gameid, int player);

        Move AssignPlayer(Move move, int player);
    }
}