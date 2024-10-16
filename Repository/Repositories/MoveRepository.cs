using Context.Interfaces;
using Models;
using Repository.Interfaces;

namespace Repository
{
    public class MoveRepository : Repository<Move>, IMoveRepository
    {
        private readonly IMoveContext context;
        public MoveRepository(IMoveContext context) : base(context)
        {
            this.context = context;
        }
    }
}