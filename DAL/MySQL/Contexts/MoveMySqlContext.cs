using System.Diagnostics.CodeAnalysis;
using Context.Interfaces;
using Models;

namespace DAL.MySQL.Contexts
{
    [ExcludeFromCodeCoverage]
    public class MoveMySqlContext : MySqlContext<Move>, IMoveContext
    {
        public MoveMySqlContext(string connectionString) : base(connectionString)
        {
        }
    }
}