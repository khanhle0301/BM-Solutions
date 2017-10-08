using BM_Solution.Model.Models;
using BM_Solution.Web.Models;
using BM_Solution.Web.Models.System;
using System;

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

        public static void UpdateAppUser(this AppUser appUser, AppUserViewModel appUserViewModel, string action = "add")
        {
            if (action == "update")
                appUser.Id = appUserViewModel.Id;
            else
                appUser.Id = Guid.NewGuid().ToString();
            appUser.FullName = appUserViewModel.FullName;
            appUser.Email = appUserViewModel.Email;
            appUser.UserName = appUserViewModel.UserName;
            appUser.Status = appUserViewModel.Status;
        }

        public static void UpdateDuAn(this DuAn duAn, DuAnViewModel duAnViewModel)
        {
            duAn.Id = duAnViewModel.Id;
            duAn.Ten = duAnViewModel.Ten;
            duAn.NgayTao = duAnViewModel.NgayTao;
            duAn.ThoiGianDuTinh = duAnViewModel.ThoiGianDuTinh;
            duAn.TienVonBanDau = duAnViewModel.TienVonBanDau;
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

        public static void UpdateChiTietThuChi(this ChiTietThuChi chiTietThuChi, ChiTietThuChiViewModel chiTietThuChiViewModel)
        {
            chiTietThuChi.Id = chiTietThuChiViewModel.Id;
            chiTietThuChi.NgayTao = chiTietThuChiViewModel.NgayTao;
            chiTietThuChi.TienChi = chiTietThuChiViewModel.TienChi;
            chiTietThuChi.TienThu = chiTietThuChiViewModel.TienThu;
            chiTietThuChi.UserId = chiTietThuChiViewModel.UserId;
            chiTietThuChi.DuAnId = chiTietThuChiViewModel.DuAnId;
            chiTietThuChi.MoreImages = chiTietThuChiViewModel.MoreImages;
        }

        public static void UpdateDuAnUser(this DuAnUser duAnUser, DuAnUserViewModel duAnUserViewModel)
        {
            duAnUser.Id = duAnUserViewModel.Id;
            duAnUser.NgayTao = duAnUserViewModel.NgayTao;
            duAnUser.TienVonBanDau = duAnUserViewModel.TienVonBanDau;
            duAnUser.UserId = duAnUserViewModel.UserId;
            duAnUser.DuAnId = duAnUserViewModel.DuAnId;
            duAnUser.IsDelete = duAnUserViewModel.IsDelete;
            duAnUser.PhanTramHoaHong = duAnUserViewModel.PhanTramHoaHong;
            duAnUser.PhanTramVon = duAnUserViewModel.PhanTramVon;
        }
    }
}