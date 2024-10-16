using System.Collections.Generic;
using Models;

namespace Logic.Interfaces
{
    public interface IUserLogic : ILogic<User>
    {
        User CreateEmtpy();
        User Create(string name, string password);
        User AssignGames(User user, List<Game> games);
        UserInfo GetUserInfo(int id);
    }
}