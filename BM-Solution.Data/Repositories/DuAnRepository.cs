using System;
using System.Collections.Generic;
using System.Linq;
using BM_Solution.Data.Infrastructure;
using BM_Solution.Model.Models;
using BM_Solutions.Common.Enums;

namespace BM_Solution.Data.Repositories
{
    public interface IDuAnRepository : IRepository<DuAn>
    {
        IEnumerable<DuAn> GetByUserId(string userId, string keyword, IEnumerable<string> role);
    }

    public class DuAnRepository : RepositoryBase<DuAn>, IDuAnRepository
    {
        public DuAnRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<DuAn> GetByUserId(string userId, string keyword, IEnumerable<string> role)
        {
            if (role.Contains(RoleEnum.Admin.ToString()))
                if (string.IsNullOrEmpty(keyword))
                    return DbContext.DuAns.Where(x => x.IsDelete == false);
                else
                    return DbContext.DuAns.Where(x => x.IsDelete == false && x.Ten.Contains(keyword) || x.Id.Contains(keyword));
            IQueryable<DuAn> query;
            if (!string.IsNullOrEmpty(keyword))
            {
                query = from d in DbContext.DuAns
                        join p in DbContext.DuAnUsers on d.Id equals p.DuaAnId
                        where p.UserId == userId && d.Ten.Contains(keyword) || d.Id.Contains(keyword) && d.IsDelete == false
                        select d;
            }
            else
            {
                query = from d in DbContext.DuAns
                        join p in DbContext.DuAnUsers on d.Id equals p.DuaAnId
                        where p.UserId == userId && d.IsDelete == false
                        select d;
            }
            return query.ToList();
        }
    }
}