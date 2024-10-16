using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Context.Interfaces;
using Models;

namespace DAL.MySQL.Contexts
{
    [ExcludeFromCodeCoverage]
    public class UserMySqlContext : MySqlContext<User>, IUserContext
    {
        public UserMySqlContext(string connectionString) : base(connectionString)
        {
        }

        public UserInfo GetUserInfo(int id)
        {
            var name = "";
            var gameCount = 0;
            var modeCounts = new Dictionary<string, int>();
            var results = Database.Procedure("GameUserNameAndGameCount", id);
            if (results.Any())
            {
                var result = results.First();
                name = result.Properties[0].Value.ToString();
                gameCount = (int)(Int64)result.Properties[1].Value;
            }
            var results2 = Database.Procedure("GetGameModesAmount", id);
            if (results2.Any())
            {
                foreach (var r in results2)
                {
                    modeCounts.Add(r.Properties[0].Value.ToString(), (int)(Int64)r.Properties[1].Value);
                }
            }
            return new UserInfo(name, gameCount, modeCounts);
        }
    }
}