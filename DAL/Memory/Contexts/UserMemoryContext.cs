using System;
using Context.Interfaces;
using Models;

namespace DAL.Memory.Contexts
{
    public class UserMemoryContext : MemoryContext<User>, IUserContext
    {
        public UserInfo GetUserInfo(int id)
        {
            throw new NotImplementedException();
        }
    }
}