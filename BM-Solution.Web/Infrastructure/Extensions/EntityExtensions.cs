using BM_Solution.Model.Models;
using BM_Solution.Web.Models.System;
using System;
using BM_Solution.Web.Models;

namespace BM_Solution.Web.Infrastructure.Extensions
{
    public static class EntityExtensions
    {
        public static void UpdateApplicationRole(this AppRole appRole, ApplicationRoleViewModel appRoleViewModel, string action = "add")
        {
            if (action == "update")
                appRole.Id = appRoleViewModel.Id;
            else
                appRole.Id = Guid.NewGuid().ToString();
            appRole.Name = appRoleViewModel.Name;
            appRole.Description = appRoleViewModel.Description;
        }

        public static void UpdateDuAn(this DuAn duAn, DuAnViewModel duAnViewModel)
        {
            duAn.Id = duAnViewModel.Id;
            duAn.Ten = duAnViewModel.Ten;
            duAn.NgayTao = DateTime.Parse(duAnViewModel.NgayTao);
            duAn.ThoiGianDuTinh = DateTime.Parse(duAnViewModel.ThoiGianDuTinh);
            duAn.LoiNhuanDuTinh = duAnViewModel.LoiNhuanDuTinh;
            duAn.TienChiDuTinh = duAnViewModel.TienChiDuTinh;
            duAn.TienChiThucTe = duAnViewModel.TienChiThucTe;
            duAn.TienThuDuTinh = duAnViewModel.TienThuDuTinh;
            duAn.TienThuThucTe = duAnViewModel.TienThuThucTe;
            duAn.LoiNhuanThucTe = duAnViewModel.LoiNhuanThucTe;
            duAn.NoiDung = duAnViewModel.NoiDung;
            duAn.GhiChu = duAnViewModel.GhiChu;
            duAn.TrangThai = duAnViewModel.TrangThai;
            duAn.IsDelete = duAnViewModel.IsDelete;

        }
    }
}