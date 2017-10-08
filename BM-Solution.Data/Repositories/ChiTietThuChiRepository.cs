using BM_Solution.Data.Infrastructure;
using BM_Solution.Model.Models;
using BM_Solutions.Common.Enums;
using BM_Solutions.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace BM_Solution.Data.Repositories
{
    public interface IChiTietThuChiRepository : IRepository<ChiTietThuChi>
    {
        IEnumerable<ChiTietThuChi> GetByDuAnId(string duaAnId, DateTime startDate, DateTime endDate);

        IEnumerable<ChiTietThuChi> NhatKyGiaoDich(IEnumerable<string> role, string userId, DateTime startDate, DateTime endDate);

        DateRange GetRangeByDuAnId(string duAnId);

        DateRange GetRange();
    }

    public class ChiTietThuChiRepository : RepositoryBase<ChiTietThuChi>, IChiTietThuChiRepository
    {
        public ChiTietThuChiRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<ChiTietThuChi> GetByDuAnId(string duaAnId, DateTime startDate, DateTime endDate)
        {
            var query = from c in DbContext.ChiTietThuChi
                        where (DbFunctions.TruncateTime(c.NgayTao) >= DbFunctions.TruncateTime(startDate))
                               && (DbFunctions.TruncateTime(c.NgayTao) <= DbFunctions.TruncateTime(endDate))
                               && (c.DuAnId == duaAnId)
                        select c;
            return query.Include("AppUser");
        }

        public DateRange GetRange()
        {
            try
            {
                return new DateRange
                {
                    MaxDate = DbContext.ChiTietThuChi.Max(t => t.NgayTao),
                    MinDate = DbContext.ChiTietThuChi.Min(t => t.NgayTao)
                };
            }
            catch
            {
                return new DateRange
                {
                    MinDate = DateTime.Now.AddDays(-1),
                    MaxDate = DateTime.Now
                };
            }
        }

        public DateRange GetRangeByDuAnId(string duAnId)
        {
            try
            {
                return new DateRange
                {
                    MaxDate = DbContext.ChiTietThuChi.Where(x => x.DuAnId == duAnId).Max(t => t.NgayTao),
                    MinDate = DbContext.ChiTietThuChi.Where(x => x.DuAnId == duAnId).Min(t => t.NgayTao)
                };
            }
            catch
            {
                return new DateRange
                {
                    MinDate = DateTime.Now.AddDays(-1),
                    MaxDate = DateTime.Now
                };
            }
        }

        public IEnumerable<ChiTietThuChi> NhatKyGiaoDich(IEnumerable<string> role, string userId, DateTime startDate, DateTime endDate)
        {
            IQueryable<ChiTietThuChi> query;

            if (role.Contains(RoleEnum.Admin.ToString()))
            {
                query = from c in DbContext.ChiTietThuChi
                        where (c.NgayTao >= startDate
                               && c.NgayTao <= endDate)
                        select c;
            }
            else
            {
                query = from c in DbContext.ChiTietThuChi
                        where (c.NgayTao >= startDate
                               && c.NgayTao <= endDate) && c.UserId == userId
                        select c;
            }
            return query.Include("AppUser");
        }
    }
}