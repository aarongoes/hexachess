using Context.Interfaces;
using Models;
using Repository.Interfaces;

namespace Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly IUserContext context;

        public UserRepository(IUserContext context) : base(context)
        {
            this.context = context;
        }

        public UserInfo GetUserInfo(int id)
        {
            return context.GetUserInfo(id);
        }
    }
}