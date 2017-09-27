using BM_Solution.Data.Infrastructure;
using BM_Solution.Model.Models;
using System.Collections.Generic;
using System.Linq;
using System;

namespace BM_Solution.Data.Repositories
{
    public interface IDuAnUserRepository : IRepository<DuAnUser>
    {
        List<string> GetUserByDuAnId(string duAnId);

        List<string> GetDuAnByUserId(string userId);

        IEnumerable<AppUser> GetByNotInDuAnId(string duAnId);

    }

    public class DuAnUserRepository : RepositoryBase<DuAnUser>, IDuAnUserRepository
    {
        public DuAnUserRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<AppUser> GetByNotInDuAnId(string duAnId)
        {
            var duAnUser = from p in DbContext.DuAnUsers
                           where p.DuAnId == duAnId && p.IsDelete == false
                           select p;

            var query = from u in DbContext.Users
                        where u.Status && !(from o in duAnUser
                                            select o.UserId).Contains(u.Id)
                        select u;
            return query.ToList();
        }

        public List<string> GetDuAnByUserId(string userId)
        {
            var query = from u in DbContext.Users
                        join p in DbContext.DuAnUsers on u.Id equals p.UserId
                        where p.UserId == userId
                        select p.DuAnId;
            return query.ToList();
        }

        public List<string> GetUserByDuAnId(string duAnId)
        {
            var query = from u in DbContext.Users
                        join p in DbContext.DuAnUsers on u.Id equals p.UserId
                        where p.DuAnId == duAnId
                        select u.UserName;
            return query.ToList();
        }
    }
}