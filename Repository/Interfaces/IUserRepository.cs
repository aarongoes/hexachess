using Models;

namespace Repository.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        UserInfo GetUserInfo(int id);
    }
}