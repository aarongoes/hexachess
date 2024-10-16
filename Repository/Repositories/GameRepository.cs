using Context.Interfaces;
using Models;
using Repository.Interfaces;

namespace Repository
{
    public class GameRepository : Repository<Game>, IGameRepository
    {
        private readonly IGameContext context;

        public GameRepository(IGameContext context) : base(context)
        {
            this.context = context;
        }
    }
}