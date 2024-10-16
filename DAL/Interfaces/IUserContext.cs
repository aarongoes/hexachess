using DAL.Interfaces;
using Models;

namespace Context.Interfaces
{
    public interface IUserContext : IContext<User>
    {
        UserInfo GetUserInfo(int id);
    }
}