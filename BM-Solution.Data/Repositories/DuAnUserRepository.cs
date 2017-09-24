using BM_Solution.Data.Infrastructure;
using BM_Solution.Model.Models;
using System.Collections.Generic;
using System.Linq;

namespace BM_Solution.Data.Repositories
{
    public interface IDuAnUserRepository : IRepository<DuAnUser>
    {
        List<string> GetUserByDuAnId(string duAnId);

        List<string> GetDuAnByUserId(string userId);
    }

    public class DuAnUserRepository : RepositoryBase<DuAnUser>, IDuAnUserRepository
    {
        public DuAnUserRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public List<string> GetDuAnByUserId(string userId)
        {
            var query = from u in DbContext.Users
                        join p in DbContext.DuAnUsers on u.Id equals p.UserId
                        where p.UserId == userId
                        select p.DuaAnId;
            return query.ToList();
        }

        public List<string> GetUserByDuAnId(string duAnId)
        {
            var query = from u in DbContext.Users
                        join p in DbContext.DuAnUsers on u.Id equals p.UserId
                        where p.DuaAnId == duAnId
                        select u.UserName;
            return query.ToList();
        }
    }
}