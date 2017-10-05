using BM_Solutions.Common.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace BM_Solutions.Common.ViewModel
{
    public class DuAnVm
    {
        [Key]
        public string Id { set; get; }

        public DateTime NgayTao { set; get; }

        public long TienVonBanDau { set; get; }

        public long? VonDauTu { set; get; }

        public float? PhanTramVon { set; get; }

        public long LoiNhuanDuTinh { set; get; }

        public long TienChiDuTinh { set; get; }

        public long TienChiThucTe { set; get; }

        public long TienThuDuTinh { set; get; }

        public long TienThuThucTe { set; get; }

        public long LoiNhuanThucTe { set; get; }

        public StatusEnum TrangThai { set; get; }
    }
}