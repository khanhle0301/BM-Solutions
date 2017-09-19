using BM_Solution.Data.Infrastructure;
using BM_Solution.Model.Models;
using System.Collections.Generic;
using System.Linq;

namespace BM_Solution.Data.Repositories
{
    public interface IPermissionRepository : IRepository<Permission>
    {
        List<Permission> GetByUserId(string userId);
    }

    public class PermissionRepository : RepositoryBase<Permission>, IPermissionRepository
    {
        public PermissionRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public List<Permission> GetByUserId(string userId)
        {
            var query = from u in DbContext.Users
                        join p in DbContext.Permissions on u.Id equals p.UserId
                        where u.Id == userId
                        select p;
            return query.ToList();
        }
    }
}