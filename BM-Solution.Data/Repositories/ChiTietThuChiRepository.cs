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
        IEnumerable<ChiTietThuChi> GetByDuAnId(string duaAnId, string startDate, string endDate);

        IEnumerable<ChiTietThuChi> NhatKyGiaoDich(IEnumerable<string> role, string userId, string startDate, string endDate);

        IEnumerable<ChiTietThuChi> DuAnThamGia(string userId, IEnumerable<string> role);

        DateRange GetRange(string duAnId, IEnumerable<string> role = null);
    }

    public class ChiTietThuChiRepository : RepositoryBase<ChiTietThuChi>, IChiTietThuChiRepository
    {
        public ChiTietThuChiRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<ChiTietThuChi> DuAnThamGia(string userId, IEnumerable<string> role)
        {
            if (role.Contains(RoleEnum.Admin.ToString()))
                return DbContext.ChiTietThuChi.Where(x => x.IsDelete == false);
            var query = from d in DbContext.ChiTietThuChi
                        where d.UserId == userId && d.IsDelete == false
                        select d;
            return query;
        }

        public IEnumerable<ChiTietThuChi> GetByDuAnId(string duaAnId, string startDate, string endDate)
        {
            DateTime dtFrom;
            DateTime dtTo;
            try
            {
                dtFrom = Convert.ToDateTime(startDate).AddDays(1);
                dtTo = Convert.ToDateTime(endDate);
            }
            catch
            {
                throw new Exception("Định dạng ngày không hợp lệ");
            }
            var query = from c in DbContext.ChiTietThuChi
                        where (DbFunctions.TruncateTime(c.NgayTao) >= DbFunctions.TruncateTime(dtFrom))
                               && (DbFunctions.TruncateTime(c.NgayTao) <= DbFunctions.TruncateTime(dtTo))
                               && (c.DuAnId == duaAnId && c.IsDelete == false)
                        select c;
            return query.Include("AppUser");
        }

        public DateRange GetRange(string duAnId, IEnumerable<string> role = null)
        {

            if (role != null)
                return new DateRange
                {
                    MaxDate = DbContext.ChiTietThuChi.Where(x => x.IsDelete == false).Max(t => t.NgayTao),
                    MinDate = DbContext.ChiTietThuChi.Where(x => x.IsDelete == false).Min(t => t.NgayTao)
                };
            return new DateRange
            {
                MaxDate = DbContext.ChiTietThuChi.Where(x => x.IsDelete == false && x.DuAnId == duAnId).Max(t => t.NgayTao),
                MinDate = DbContext.ChiTietThuChi.Where(x => x.IsDelete == false && x.DuAnId == duAnId).Min(t => t.NgayTao)
            };
        }

        public IEnumerable<ChiTietThuChi> NhatKyGiaoDich(IEnumerable<string> role, string userId, string startDate, string endDate)
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

            if (role.Contains(RoleEnum.Admin.ToString()))
                return DbContext.ChiTietThuChi.Where(x => x.IsDelete == false).Include("AppUser");
            var query = from c in DbContext.ChiTietThuChi
                        where (c.NgayTao >= dtFrom
                               && c.NgayTao <= dtTo) && c.IsDelete == false && c.UserId == userId
                        select c;
            return query.Include("AppUser");
        }
    }
}