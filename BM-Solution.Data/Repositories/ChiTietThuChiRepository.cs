using BM_Solution.Data.Infrastructure;
using BM_Solution.Model.Models;

namespace BM_Solution.Data.Repositories
{
    public interface IChiTietThuChiRepository : IRepository<ChiTietThuChi>
    {
    }

    public class ChiTietThuChiRepository : RepositoryBase<ChiTietThuChi>, IChiTietThuChiRepository
    {
        public ChiTietThuChiRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}