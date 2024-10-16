using System.Collections.Generic;
using System.Linq;
using Logic.Interfaces;
using Models;

namespace Logic
{
    public class ApplicationLogic : IApplicationLogic
    {
        private readonly IGameLogic gameLogic;
        private readonly IMoveLogic moveLogic;
        private readonly IBoardLogic boardLogic;
        private readonly IUserLogic userLogic;

        public ApplicationLogic(IGameLogic gameLogic, IMoveLogic moveLogic, IBoardLogic boardLogic, IUserLogic userLogic)
        {
            this.gameLogic = gameLogic;
            this.moveLogic = moveLogic;
            this.userLogic = userLogic;
            this.boardLogic = boardLogic;
        }
        
        public Game LoadGame(string token, string user)
        {
            Game game;
            game = gameLogic.Read(new KeyValuePair<string, object>("Token", token));
            var moves = moveLogic.ReadAll(new Dictionary<string, object>{{"GameId", game.Id}});
            game = gameLogic.AssignMoves(game, moves);
            var player1 = userLogic.Read(new KeyValuePair<string, object>("Id", game.FirstPlayerId));
            User player2 = null;
            if (game.SecondPlayerId != null)
            {
                player2 = userLogic.Read(new KeyValuePair<string, object>("Id", game.SecondPlayerId));
            }
            game = gameLogic.AssignPlayers(game, player1, player2);
            return game;
        }

        public Game CreateGame(string user, int mode)
        {
            var player = userLogic.Read(new KeyValuePair<string, object>("Token", user));
            Game game = gameLogic.Add(mode, player.Id);
            game = gameLogic.AssignPlayers(game, player, null);
            return game;
        }
        
        public string JoinGame(string token, string user)
        {
            Game game = gameLogic.Read(new KeyValuePair<string, object>("Token", token));
            var player2 = userLogic.Read(new KeyValuePair<string, object>("Token", user));
            if (game.SecondPlayerId == null && game.Mode == 2 && game.FirstPlayerId != player2.Id)
            {
                gameLogic.Update(new KeyValuePair<string, object>("Token", game.Token), new KeyValuePair<string, object>("SecondPlayerId", player2.Id));
                return game.Token.ToString();
            }
            return null;
        }

        public Board MovePiece(Move chessMove, string gameToken, string userToken)
        {
            var game = gameLogic.Read(new KeyValuePair<string, object>("Token", gameToken));
            var player = userLogic.Read(new KeyValuePair<string, object>("Token", userToken));
            var moves = moveLogic.ReadAll(new Dictionary<string, object>{{"GameId", game.Id}});
            game = gameLogic.AssignMoves(game, moves);
            var returnBoard = boardLogic.LoadBoard(game);
            if (isPlayerAllowedToPerformMove(game, player, returnBoard, chessMove))
            {
                var move = moveLogic.Create(chessMove.OldX, chessMove.OldY, chessMove.NewX, chessMove.NewY, game.Id, boardLogic.GetPlayerAssignedToPiecePosition(returnBoard, chessMove.OldX,chessMove.OldY));
                returnBoard = boardLogic.ApplyMove(returnBoard, move);
                if (returnBoard.Succes)moveLogic.Add(returnBoard.Game.Moves.Last());
            }
            return returnBoard;
        }

        private bool isPlayerAssignedToGame(Game game, User player)
        {
            return player.Id == game.FirstPlayerId || player.Id == game.SecondPlayerId;
        }

        private bool isPlayerAllowedToMovePiece(Game game, User player, int ownerOfPiece)
        {
            return player.Id == game.FirstPlayerId && ownerOfPiece == 1 ||
                   player.Id == game.SecondPlayerId && ownerOfPiece == 2 ||
                   player.Id == game.FirstPlayerId && ownerOfPiece == 2 && game.Mode == 1;
        }

        private bool hasOptionalSecondPlayerJoined(Game game)
        {
            return game.SecondPlayerId != null || game.Mode != 2;
        }

        private bool isPlayerAllowedToPerformMove(Game game, User player, Board returnBoard, Move chessMove)
        {
            return isPlayerAssignedToGame(game, player) && isPlayerAllowedToMovePiece(game, player,
                       boardLogic.GetPlayerAssignedToPiecePosition(returnBoard, chessMove.OldX, chessMove.OldY)) &&
                   hasOptionalSecondPlayerJoined(game);
        }

        public void Save(string value, string token)
        {
            throw new System.NotImplementedException();
        }

        public Board LoadBoard(string gameToken)
        {
            throw new System.NotImplementedException();
        }

        public Game LoadStats(string gameToken)
        {
            throw new System.NotImplementedException();
        }
    }
}