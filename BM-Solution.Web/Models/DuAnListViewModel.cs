using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BM_Solution.Web.Models
{
    public class DuAnListViewModel
    {
        public string Id { set; get; }

        [Required]
        [MaxLength(100)]
        public string Ten { set; get; }

        public DateTime? NgayTao { set; get; }

        public DateTime? ThoiGianDuTinh { set; get; }

        public Decimal? LoiNhuanDuTinh { set; get; }

        public Decimal? TienChiDuTinh { set; get; }

        public Decimal? TienChiThucTe { set; get; }

        public Decimal? TienThuDuTinh { set; get; }

        public Decimal? TienThuThucTe { set; get; }

        public Decimal? LoiNhuanThucTe { set; get; }

        [MaxLength(500)]
        public string NoiDung { set; get; }

        [MaxLength(100)]
        public string GhiChu { set; get; }

        public int TrangThai { set; get; }
        public bool IsDelete { get; set; }

        public ICollection<string> AppUser { get; set; }
    }
}