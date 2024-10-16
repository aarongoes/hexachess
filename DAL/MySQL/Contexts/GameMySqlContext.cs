using System.Diagnostics.CodeAnalysis;
using Context.Interfaces;
using Models;

namespace DAL.MySQL.Contexts
{
    [ExcludeFromCodeCoverage]
    public class GameMySqlContext : MySqlContext<Game>, IGameContext
    {
        public GameMySqlContext(string connectionString) : base(connectionString)
        {
        }
    }
}