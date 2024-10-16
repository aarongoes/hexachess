using System;
using System.Collections.Generic;
using Logic.Interfaces;
using Repository.Interfaces;
using Models;

namespace Logic
{
    public class GameLogic : Logic<Game>, IGameLogic
    {
        private readonly IGameRepository repository;

        public GameLogic(IGameRepository repository) : base(repository)
        {
            this.repository = repository;
        }

        public Game Add(int mode, int firstUserId)
        {
            var game = new Game(Guid.NewGuid(), mode, firstUserId);
            return repository.Add(game);
        }

        public Game AssignMoves(Game game, List<Move> moves)
        {
            game.Moves = new List<Move>();
            if (moves != null)
            {
                game.Moves.AddRange(moves);
            }
            return game;
        }
        
        public Game AssignPlayers(Game game, User firstPlayer, User secondPlayer)
        {
            game.FirstPlayer = firstPlayer;
            game.SecondPlayer = secondPlayer;
            return game;
        }

        public Game AssignThumbnail(Game game, byte[] thumbnail)
        {
            game.Thumbnail = thumbnail;
            return game;
        }
        
        public Game AddSecondPlayer(Game game, User user)
        {
            game.SecondPlayerId = user.Id;
            return game;
        }
    }
}