using BM_Solution.Data.Infrastructure;
using BM_Solution.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BM_Solution.Data.Repositories
{
    public interface ISystemLogRepository : IRepository<SystemLog>
    {
        IEnumerable<SystemLog> NhatKyHeThong(string startDate, string endDate);
    }

    public class SystemLogRepository : RepositoryBase<SystemLog>, ISystemLogRepository
    {
        public SystemLogRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<SystemLog> NhatKyHeThong(string startDate, string endDate)
        {
            DateTime dtFrom;
            DateTime dtTo;
            try
            {
                dtFrom = Convert.ToDateTime(startDate).AddDays(1);
                dtTo = Convert.ToDateTime(endDate).AddDays(1);
            }
            catch
            {
                throw new Exception("Định dạng ngày không hợp lệ");
            }
            var query = from c in DbContext.SystemLogs
                        where (c.NgayTao >= dtFrom
                       && c.NgayTao <= dtTo) && c.IsDelete == false
                        select c;
            return query;
        }
    }
}