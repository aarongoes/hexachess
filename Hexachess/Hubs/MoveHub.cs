using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Logic.Interfaces;
using Models.Enums;

namespace Hexachess.Hubs
{
    [ExcludeFromCodeCoverage]
    public class MoveHub : Hub
    {
        private readonly IGameLogic gameLogic;
        private readonly IMoveLogic moveLogic;
        private readonly IBoardLogic boardLogic;
        private readonly IUserLogic userLogic;
        private readonly IApplicationLogic applicationLogic;

        public MoveHub(global::Factory.Factory factory)
        {
            gameLogic = factory.GetGameLogic(Engine.MySql);
            moveLogic = factory.GetMoveLogic(Engine.MySql);
            userLogic = factory.GetUserLogic(Engine.MySql);
            boardLogic = factory.GetBoardLogic(factory.GetHexagonLogic());
            applicationLogic = factory.GetApplicationLogic(gameLogic, moveLogic, boardLogic, userLogic);
        }
        public async Task JoinRoom(string token)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, token);
        }
    }
}