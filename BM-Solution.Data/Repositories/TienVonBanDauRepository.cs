using BM_Solution.Data.Infrastructure;
using BM_Solution.Model.Models;

namespace BM_Solution.Data.Repositories
{
    public interface ITienVonBanDauRepository : IRepository<TienVonBanDau>
    {
    }

    public class TienVonBanDauRepository : RepositoryBase<TienVonBanDau>, ITienVonBanDauRepository
    {
        public TienVonBanDauRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}