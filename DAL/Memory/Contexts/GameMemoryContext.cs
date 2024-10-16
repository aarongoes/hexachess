using Context.Interfaces;
using Models;

namespace DAL.Memory.Contexts
{
    public class GameMemoryContext : MemoryContext<Game>, IGameContext
    {
        
    }
}