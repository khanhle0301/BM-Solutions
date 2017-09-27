using BM_Solutions.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BM_Solution.Web.Models
{
    public class DuAnViewModel
    {
        [Required]
        [MaxLength(50)]
        public string Id { set; get; }

        [Required]
        [MaxLength(100)]
        public string Ten { set; get; }

        [Required]
        public DateTime NgayTao { set; get; }

        [Required]
        public DateTime ThoiGianDuTinh { set; get; }

        public long TienVonBanDau { set; get; }

        public long LoiNhuanDuTinh { set; get; }

        public long TienChiDuTinh { set; get; }

        public long TienChiThucTe { set; get; }

        public long TienThuDuTinh { set; get; }

        public long TienThuThucTe { set; get; }

        public long LoiNhuanThucTe { set; get; }

        [MaxLength(500)]
        public string NoiDung { set; get; }

        [MaxLength(100)]
        public string GhiChu { set; get; }

        public StatusEnum TrangThai { set; get; }
        public bool IsDelete { get; set; }

        public IEnumerable<DuAnUserViewModel> DuAnUserViewModels { get; set; }
    }
}