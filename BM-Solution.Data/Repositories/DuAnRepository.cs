using BM_Solution.Data.Infrastructure;
using BM_Solution.Model.Models;

namespace BM_Solution.Data.Repositories
{
    public interface IDuAnRepository : IRepository<DuAn>
    {
    }

    public class DuAnRepository : RepositoryBase<DuAn>, IDuAnRepository
    {
        public DuAnRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}