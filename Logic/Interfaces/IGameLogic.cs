using System.Collections.Generic;
using Models;

namespace Logic.Interfaces
{
    public interface IGameLogic : ILogic<Game>
    {
        Game Add(int mode, int firstUserId);

        Game AssignMoves(Game game, List<Move> moves);

        Game AssignPlayers(Game game, User firstPlayer, User secondPlayer);
        
        Game AssignThumbnail(Game game, byte[] thumbnail);
        
        Game AddSecondPlayer(Game game, User user);
    }
}