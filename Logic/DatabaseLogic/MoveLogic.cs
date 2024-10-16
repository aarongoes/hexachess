using Logic.Interfaces;
using Models;
using Repository.Interfaces;

namespace Logic
{
    public class MoveLogic : Logic<Move>, IMoveLogic
    {
        private readonly IMoveRepository repository;

        public MoveLogic(IMoveRepository repository) : base(repository)
        {
            this.repository = repository;
        }

        public Move Create(decimal oldX, decimal oldY, decimal newX, decimal newY, int gameid, int player)
        {
            var move = new Move(oldX, oldY, newX, newY, gameid, player);
            return move;
        }

        public Move AssignPlayer(Move move, int player)
        {
            move.Player = player;
            return move;
        }
    }
}