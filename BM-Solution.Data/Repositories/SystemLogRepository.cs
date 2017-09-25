using BM_Solution.Data.Infrastructure;
using BM_Solution.Model.Models;
using BM_Solutions.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace BM_Solution.Data.Repositories
{
    public interface ISystemLogRepository : IRepository<SystemLog>
    {
        IEnumerable<SystemLog> NhatKyHeThong(DateTime startDate, DateTime endDate);

        DateRange GetRange();
    }

    public class SystemLogRepository : RepositoryBase<SystemLog>, ISystemLogRepository
    {
        public SystemLogRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<SystemLog> NhatKyHeThong(DateTime startDate, DateTime endDate)
        {
            var query = from c in DbContext.SystemLogs
                        where (DbFunctions.TruncateTime(c.NgayTao) >= DbFunctions.TruncateTime(startDate))
                               && (DbFunctions.TruncateTime(c.NgayTao) <= DbFunctions.TruncateTime(endDate))
                               && (c.IsDelete == false)
                        select c;
            return query;
        }

        public DateRange GetRange()
        {
            return new DateRange
            {
                MaxDate = DbContext.SystemLogs.Where(x => x.IsDelete == false).Max(t => t.NgayTao),
                MinDate = DbContext.SystemLogs.Where(x => x.IsDelete == false).Min(t => t.NgayTao)
            };
        }
    }
}