using System;
using System.Collections.Generic;
using DAL.Memory.Contexts;
using DAL.MySQL.Contexts;
using Logic;
using Logic.Interfaces;
using Models.Enums;
using Repository;

namespace Factory
{
    public class Factory
    {
        private readonly Dictionary<Engine, string> _connectionStrings;


        public Factory(Dictionary<Engine, string> connectionStrings)
        {
            _connectionStrings = connectionStrings;
        }

        public IGameLogic GetGameLogic(Engine engine)
        {
            switch (engine)
            {
                case Engine.MySql:
                    return new GameLogic(new GameRepository(new GameMySqlContext(_connectionStrings[engine])));
                case Engine.Memory:
                    return new GameLogic(new GameRepository(new GameMemoryContext()));
                default: throw new NotImplementedException();
            }
        }

        public IMoveLogic GetMoveLogic(Engine engine)
        {
            switch (engine)
            {
                case Engine.MySql:
                    return new MoveLogic(new MoveRepository(new MoveMySqlContext(_connectionStrings[engine])));
                case Engine.Memory:
                    return new MoveLogic(new MoveRepository(new MoveMemoryContext()));
                default:
                    throw new NotImplementedException();
            }
        }

        public IUserLogic GetUserLogic(Engine engine)
        {
            switch (engine)
            {
                case Engine.MySql:
                    return new UserLogic(new UserRepository(new UserMySqlContext(_connectionStrings[engine])));
                case Engine.Memory:
                    return new UserLogic(new UserRepository(new UserMemoryContext()));
                default:
                    throw new NotImplementedException();
            }
        }

        public IBoardLogic GetBoardLogic(IHexagonLogic hexagonLogic)
        {
            return new BoardLogic(hexagonLogic);
        }

        public IHexagonLogic GetHexagonLogic()
        {
            return new HexagonLogic();
        }

        public IApplicationLogic GetApplicationLogic(IGameLogic gameLogic, IMoveLogic moveLogic, IBoardLogic boardLogic, IUserLogic userLogic)
        {
            return new ApplicationLogic(gameLogic, moveLogic, boardLogic, userLogic);
        }
    }
}