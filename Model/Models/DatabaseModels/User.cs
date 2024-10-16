using System;
using System.Collections.Generic;
using Models.Utils;

namespace Models
{
    public class User : DatabaseItem
    {
        [Property] public Guid Token { get; private set; }
        [Property] public string Name { get; private set; }
        [Property] public string Password { get; private set; }
        public List<Game> Games { get; set; }
        
        public User()
        {
        }
        
        public User(Guid token, string name, string password)
        {
            Token = token;
            Name = name;
            Password = password;
            Games = new List<Game>();
        }
    }
}