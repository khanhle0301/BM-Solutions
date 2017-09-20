using BM_Solution.Data.Infrastructure;
using BM_Solution.Model.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System;

namespace BM_Solution.Data.Repositories
{
    public interface IPermissionRepository : IRepository<DuAnUser>
    {
        List<string> GetUserByDuAnId(string duAnId);
        List<DuAnUser> GetDuAnByUserId(string userId);
    }

    public class DuAnUserRepository : RepositoryBase<DuAnUser>, IPermissionRepository
    {
        public DuAnUserRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public List<DuAnUser> GetDuAnByUserId(string userId)
        {
            var query = from u in DbContext.Users
                        join p in DbContext.DuAnUsers on u.Id equals p.UserId
                        where p.UserId == userId
                        select p;
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