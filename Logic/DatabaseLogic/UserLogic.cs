using System;
using System.Collections.Generic;
using Logic.Interfaces;
using Repository.Interfaces;
using Models;

namespace Logic
{
    public class UserLogic : Logic<User>, IUserLogic
    {
        private readonly IUserRepository repository;

        public UserLogic(IUserRepository repository) : base(repository)
        {
            this.repository = repository;
        }

        public User CreateEmtpy()
        {
            return new User();
        }
        
        public User Create(string name, string password)
        {
            return new User(Guid.NewGuid(), name, password);
        }

        public User AssignGames(User user, List<Game> games)
        {
            user.Games = new List<Game>();
            if (games != null)
            {
                user.Games.AddRange(games);
            }
            return user;
        }

        public UserInfo GetUserInfo(int id)
        {
            return repository.GetUserInfo(id);
        }
    }
}