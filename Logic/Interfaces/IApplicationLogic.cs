using Models;

namespace Logic.Interfaces
{
    public interface IApplicationLogic
    {
        Game LoadGame(string token, string user);
        Game CreateGame(string user, int mode);
        Board MovePiece(Move chessMove, string gameToken, string userToken);
        string JoinGame(string token, string user);
        void Save(string value, string token);
        Board LoadBoard(string gameToken);
        Game LoadStats(string gameToken);
    }
}