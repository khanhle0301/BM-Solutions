using BM_Solutions.Common.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BM_Solution.Model.Models
{
    [Table("DuAns")]
    public class DuAn
    {
        [Key]
        [StringLength(50)]
        [Column(TypeName = "varchar")]
        public string Id { set; get; }

        [Required]
        [MaxLength(100)]
        public string Ten { set; get; }

        public DateTime NgayTao { set; get; }
        public DateTime ThoiGianDuTinh { set; get; }

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
    }
}