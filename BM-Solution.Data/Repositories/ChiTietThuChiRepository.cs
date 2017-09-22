using System;
using System.Collections.Generic;
using System.Data.Entity;
using BM_Solution.Data.Infrastructure;
using BM_Solution.Model.Models;
using System.Linq;

namespace BM_Solution.Data.Repositories
{
    public interface IChiTietThuChiRepository : IRepository<ChiTietThuChi>
    {
        IEnumerable<ChiTietThuChi> GetByDuAnId(string duaAnId, string startDate, string endDate);
    }

    public class ChiTietThuChiRepository : RepositoryBase<ChiTietThuChi>, IChiTietThuChiRepository
    {
        public ChiTietThuChiRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<ChiTietThuChi> GetByDuAnId(string duaAnId, string startDate, string endDate)
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
            var query = from c in DbContext.ChiTietThuChi
                        where (c.NgayTao >= dtFrom
                               && c.NgayTao <= dtTo) && c.DuAnId == duaAnId
                        select c;
            return query.Include("AppUser");
        }
    }
}